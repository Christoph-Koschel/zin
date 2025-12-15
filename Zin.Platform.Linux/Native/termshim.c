#include <termios.h>
#include <unistd.h>

struct termios orig_termios;

int term_exit_raw_mode(void)
{
    if (tcsetattr(STDIN_FILENO, TCSAFLUSH, &orig_termios) == -1)
    {
        return 0;
    }

    return 1;
}

int term_enter_raw_mode(void)
{
    if (tcgetattr(STDIN_FILENO, &orig_termios) == -1)
    {
        return 0;
    }

    struct termios raw = orig_termios;
    raw.c_iflag &= ~(BRKINT | ICRNL | INPCK | ISTRIP | IXON);
    raw.c_oflag &= ~(OPOST);
    raw.c_cflag |= (CS8);
    raw.c_lflag &= ~(ECHO | ICANON | IEXTEN | ISIG);
    raw.c_cc[VMIN] = 0;
    raw.c_cc[VTIME] = 1;

    if (tcsetattr(STDIN_FILENO, TCSAFLUSH, &raw) == -1)
    {
        return 0;
    }

    return 1;
}

int term_read(char* c_out)
{
    return read(STDIN_FILENO, c_out, 1) == 1;
}

void term_write(char* str, int count)
{
    write(STDOUT_FILENO, str, count);
}
