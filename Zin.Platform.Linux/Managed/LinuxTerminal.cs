using System;
using Zin.Platform.Base;

namespace Zin.Platform.Linux.Managed;

public sealed class LinuxTerminal : ITerminal
{
    public int Width => Console.WindowWidth;
    public int Height => Console.WindowHeight;

    public LinuxTerminal()
    {
    }

    public void EnableRawMode()
    {
        if (TermShim.EnterRawMode() != 1)
        {
            throw new Exception("Fail to enable raw mode");
        }
    }

    public void DisableRawMode()
    {
        if (TermShim.ExitRawMode() != 1)
        {
            throw new Exception("Fail to disable raw mode");
        }
    }

    public InputChar Read()
    {
        if (TermShim.Read(out byte c) != 1)
        {
            return new InputChar(0);
        }

        if (c == '\x1b')
        {
            return new InputChar(ParseVT100(), true);
        }

        return new InputChar(c);
    }

    public void Write(string text) => TermShim.Write(text, text.Length);

    public void Dispose()
    {
        DisableRawMode();
    }

    private InputChar.EscapeCode ParseVT100()
    {
        if (TermShim.Read(out byte seq0) != 1)
        {
            return InputChar.EscapeCode.Escape;
        }

        if (TermShim.Read(out byte seq1) != 1)
        {
            return InputChar.EscapeCode.Escape;
        }

        if (seq0 == '[')
        {
            if (seq1 >= '0' && seq1 <= '9')
            {

                if (TermShim.Read(out byte seq2) != 1)
                {
                    return InputChar.EscapeCode.Escape;
                }

                if (seq2 == '~')
                {
                    return seq1 switch
                    {
                        (byte)'1' => InputChar.EscapeCode.Home,
                        (byte)'3' => InputChar.EscapeCode.Delete,
                        (byte)'4' => InputChar.EscapeCode.End,
                        (byte)'5' => InputChar.EscapeCode.PageUp,
                        (byte)'6' => InputChar.EscapeCode.PageDown,
                        (byte)'7' => InputChar.EscapeCode.Home,
                        (byte)'8' => InputChar.EscapeCode.End,
                        _ => InputChar.EscapeCode.Escape
                    };
                }

                return seq1 switch
                {
                    (byte)'A' => InputChar.EscapeCode.ArrowUp,
                    (byte)'B' => InputChar.EscapeCode.ArrowDown,
                    (byte)'C' => InputChar.EscapeCode.ArrowRight,
                    (byte)'D' => InputChar.EscapeCode.ArrowLeft,
                    (byte)'H' => InputChar.EscapeCode.Home,
                    (byte)'F' => InputChar.EscapeCode.End,
                    _ => InputChar.EscapeCode.Escape
                };
            }
            
            return InputChar.EscapeCode.Escape;
        }

        if (seq0 == 'O')
        {
            return seq1 switch
            {
                (byte)'A' => InputChar.EscapeCode.ArrowUp,
                (byte)'B' => InputChar.EscapeCode.ArrowDown,
                (byte)'C' => InputChar.EscapeCode.ArrowRight,
                (byte)'D' => InputChar.EscapeCode.ArrowLeft,
                (byte)'H' => InputChar.EscapeCode.Home,
                (byte)'F' => InputChar.EscapeCode.End,
                _ => InputChar.EscapeCode.Escape
            };
        }

        return InputChar.EscapeCode.Escape;
    }
}