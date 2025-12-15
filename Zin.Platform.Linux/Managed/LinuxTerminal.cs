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
			if (!TermShim.Read(out byte seq0))
			{
				return new InputChar(InputChar.EscapeCode.Escape, true);
			}
			if (!TermShim.Read(out byte seq1))
			{
				return new InputChar(InputChar.EscapeCode.Escape, true);
			}

			if (seq0 == '[')
			{
				return new InputChar(InputChar.EscapeCode.Escape, true);
			}

			if (seq1 >= '0' && seq1 <= '9')
			{
				if (!TermShim.Read(out byte seq2))
				{ 
					return new InputChar(InputChar.EscapeCode.Escape, true);
				}

				if (seq2 == '~') {
					switch(seq1) 
					{
						case (byte)'5':
							return new InputChar(InputChar.EscapeCode.PageUp, true);
						case (byte)'6':
							return new InputChar(InputChar.EscapeCode.PageDown, true);
					}

					return new InputChar(InputChar.EscapeCode.Escape, true);
				}
			}

			switch (seq1)
			{
				case (byte)'A':
					return new InputChar(InputChar.EscapeCode.ArrowUp, true);
				case (byte)'B':
					return new InputChar(InputChar.EscapeCode.ArrowDown, true);
				case (byte)'C':
					return new InputChar(InputChar.EscapeCode.ArrowRight, true);
				case (byte)'D':
					return new InputChar(InputChar.EscapeCode.ArrowLeft, true);
			}
		}

        return new InputChar(c);
    }

    public void Write(string text) => TermShim.Write(text, text.Length);

    public void Dispose()
    {
        DisableRawMode();
    }
}
