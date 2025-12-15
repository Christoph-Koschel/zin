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
        if (!TermShim.EnterRawMode())
        {
            throw new Exception("Fail to enable raw mode");
        }
    }

    public void DisableRawMode()
    {
        if (!TermShim.ExitRawMode())
        {
            throw new Exception("Fail to disable raw mode");
        }
    }

    public InputChar Read()
    {
        if (!TermShim.Read(out byte c))
        {
            return new InputChar(0);
        }

		if (c == '\x1b')
		{
			return VT100Parser.Parse(this);
		}

        return new InputChar(c);
    }

    public void Write(string text) => TermShim.Write(text, text.Length);

    public void Dispose()
    {
        DisableRawMode();
    }
}
