#include <windows.h>

#if defined(_MSC_VER)
#  define DLL_EXPORT __declspec(dllexport)
#else
#  define DLL_EXPORT __attribute__((dllexport))
#endif

static HANDLE h_in = NULL;
static HANDLE h_out = NULL;
static DWORD orig_mode_in = 0;

// Order must be in sync with InputChar.EscapeCode
typedef enum {
    EC_ESCAPE = 256,

    EC_ARROW_UP,
    EC_ARROW_DOWN,
    EC_ARROW_LEFT,
    EC_ARROW_RIGHT,
   
    EC_PAGE_UP,
    EC_PAGE_DOWN,
    EC_DELETE,
    EC_END,
    EC_HOME
} EscapeCode;

DLL_EXPORT int term_enter_raw_mode(void)
{
    if (h_in == NULL)
    {
        h_in = GetStdHandle(STD_INPUT_HANDLE);
    }
    if (h_out == NULL)
    {
        h_out = GetStdHandle(STD_OUTPUT_HANDLE);
    }
    if (h_in == INVALID_HANDLE_VALUE || h_out == INVALID_HANDLE_VALUE)
    {
        return 0;
    }

    if (!GetConsoleMode(h_in, &orig_mode_in))
    {
        return 0;
    }

    DWORD raw = orig_mode_in;
    
    raw &= ~(ENABLE_LINE_INPUT |
         ENABLE_ECHO_INPUT |
         ENABLE_PROCESSED_INPUT |
         ENABLE_QUICK_EDIT_MODE);
    raw |= ENABLE_EXTENDED_FLAGS;
    
    if (!SetConsoleMode(h_in, raw))
    {
        return 0;
    }

    return 1;
}

DLL_EXPORT int term_exit_raw_mode(void)
{
    if (h_in == NULL || h_in == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    if (!SetConsoleMode(h_in, orig_mode_in))
    {
        return 0;
    }
    return 1;
}

static int handle_key_event(KEY_EVENT_RECORD key, unsigned short * c_out)
{
    if (!key.bKeyDown)
    {
        return 0;
    }

    if (key.uChar.AsciiChar)
    {
        unsigned char ascii_key = key.uChar.AsciiChar;

        switch (ascii_key)
        {
        case '\x1b':
            *c_out = EC_ESCAPE;
            return 1;
        default:
            *c_out = ascii_key;
            return 1;
        }
    }

    switch (key.wVirtualKeyCode)
    {
    case VK_ESCAPE:
        *c_out = EC_ESCAPE;
        return 1;
    case VK_UP:
        *c_out = EC_ARROW_UP;
        return 1;
    case VK_DOWN:
        *c_out = EC_ARROW_DOWN;
        return 1;
    case VK_LEFT:
        *c_out = EC_ARROW_LEFT;
        return 1;
    case VK_RIGHT:
        *c_out = EC_ARROW_RIGHT;
        return 1;
    case VK_PRIOR:
        *c_out = EC_PAGE_UP;
        return 1;
    case VK_NEXT:
        *c_out = EC_PAGE_DOWN;
        return 1;
    case VK_DELETE:
        *c_out = EC_DELETE;
        return 1;
    case VK_END:
        *c_out = EC_END;
        return 1;
    case VK_HOME:
        *c_out = EC_HOME;
        return 1;
    default:
        return 0;
    }
}

DLL_EXPORT int term_read(unsigned short * c_out)
{
    if (h_in == NULL)
    {
        h_in = GetStdHandle(STD_INPUT_HANDLE);
    }
    if (h_in == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    
    INPUT_RECORD rec;
    DWORD read;

    if (!GetNumberOfConsoleInputEvents(h_in, &read) || read == 0)
    {
        return 0;
    }
 
    if (!ReadConsoleInput(h_in, &rec, 1, &read))
    {
        return 0;
    }

    switch (rec.EventType)
    {
    case KEY_EVENT:
        return handle_key_event(rec.Event.KeyEvent, c_out);
    default:
        return 0;
    }
}

DLL_EXPORT int term_write(const char* str, int count)
{
    if (h_out == NULL)
    {
        h_out = GetStdHandle(STD_OUTPUT_HANDLE);
    }
    if (h_out == INVALID_HANDLE_VALUE)
    {
        return 0;
    }
    DWORD written = 0;
    if (!WriteFile(h_out, str, (DWORD)count, &written, NULL))
    {
        return 0;
    }
    return written == (DWORD)count;
}
