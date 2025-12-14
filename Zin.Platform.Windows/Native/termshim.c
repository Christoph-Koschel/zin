#include <windows.h>

#if defined(_MSC_VER)
#  define TERM_EXPORT __declspec(dllexport)
#else
#  define TERM_EXPORT __attribute__((dllexport))
#endif

static HANDLE hIn = NULL;
static HANDLE hOut = NULL;
static DWORD origModeIn = 0;

TERM_EXPORT int term_enter_raw_mode(void)
{
    if (hIn == NULL)
    {
        hIn = GetStdHandle(STD_INPUT_HANDLE);
    }
    if (hOut == NULL)
    {
        hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    }
    if (hIn == INVALID_HANDLE_VALUE || hOut == INVALID_HANDLE_VALUE)
    {
        return 0;
    }

    if (!GetConsoleMode(hIn, &origModeIn))
    {
        return 0;
    }

    DWORD raw = origModeIn;
    raw &= ~(ENABLE_ECHO_INPUT | ENABLE_LINE_INPUT | ENABLE_PROCESSED_INPUT);
    raw |= ENABLE_EXTENDED_FLAGS; // ensure extended flags set
    if (!SetConsoleMode(hIn, raw))
    {
        return 0;
    }

    return 1;
}

TERM_EXPORT int term_exit_raw_mode(void)
{
    if (hIn == NULL || hIn == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    if (!SetConsoleMode(hIn, origModeIn))
    {
        return 0;
    }
    return 1;
}

TERM_EXPORT int term_read(char* c_out)
{
    if (hIn == NULL)
    {
        hIn = GetStdHandle(STD_INPUT_HANDLE);
    }
    if (hIn == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    DWORD read = 0;
    if (!ReadFile(hIn, c_out, 1, &read, NULL))
    {
        return 0;
    }
    return read == 1;
}

TERM_EXPORT int term_write(const char* str, int count)
{
    if (hOut == NULL)
    {
        hOut = GetStdHandle(STD_OUTPUT_HANDLE);
    }
    if (hOut == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    DWORD written = 0;
    if (!WriteFile(hOut, str, (DWORD)count, &written, NULL))
    {
        return 0;
    }
    return written == (DWORD)count;
}
