using Zin.Platform.Base;

namespace Zin.Platform.Linux.Managed;

public static class VT100Parser
{
    public static InputChar Parse()
    {
        InputChar input;

        if (!TermShim.Read(out byte seq0) ||
            seq0 != '[' ||
            !TermShim.Read(out byte seq1))
        {
            return new InputChar(InputChar.EscapeCode.Escape, true);
        }

        if (TryParseNumberCommand(seq1, out input) ||
            TryParseLetterCommands(seq1, out input))
        {
            return input;
        }

        return new InputChar(InputChar.EscapeCode.Escape, true);
    }

    private static bool TryParseNumberCommand(byte seq1, out InputChar input)
    {
        if (seq1 < '0' || seq1 > '9' ||
            !TermShim.Read(out byte seq2) ||
            seq2 != '~')
        {
            input = null;
            return false;
        }

        switch (seq1)
        {
            case (byte)'5':
                input = new InputChar(InputChar.EscapeCode.PageUp, true);
                return true;
            case (byte)'6':
                input = new InputChar(InputChar.EscapeCode.PageDown, true);
                return true;
        }

        input = null;
        return false;
    }

    private static bool TryParseLetterCommands(byte seq1, out InputChar input)
    {
        switch (seq1)
        {
            case (byte)'A':
                input = new InputChar(InputChar.EscapeCode.ArrowUp, true);
                return true;
            case (byte)'B':
                input = new InputChar(InputChar.EscapeCode.ArrowDown, true);
                return true;
            case (byte)'C':
                input = new InputChar(InputChar.EscapeCode.ArrowRight, true);
                return true;
            case (byte)'D':
                input = new InputChar(InputChar.EscapeCode.ArrowLeft, true);
                return true;
        }

        input = null;
        return false;
    }
}