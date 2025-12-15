namespace Zin.Platform.Base;

public static class VT100Parser
{
	public static InputChar Parse(ITerminal terminal)
	{
		InputChar input;
		
		if (!terminal.Read(out byte seq0))
		{
			return new InputChar(InputChar.EscapeCode.Escape, true);
		}

		if (seq0 != '[')
		{		
			return new InputChar(InputChar.EscapeCode.Escape, true);
		}
		
		if (!terminal.Read(out byte seq1))
		{
			return new InputChar(InputChar.EscapeCode.Escape, true);
		}
		
		if (TryParseNumberCommand(terminal, seq1, out input))
		{
			return input;
		}

		if (TryParseLetterCommands(seq1, out input))
		{
			return input;
		}

		return new InputChar(InputChar.EscapeCode.Escape, true);
	}

	private static bool TryParseNumbeCommand(ITerminal terminal, byte seq1, out InputChar input)
	{
		if (seq1 < '0' && seq1 > '9')
		{
			input = null;
			return false;
		}

		if (!terminal.Read(out byte seq2))
		{
			input = null;
			return false;
		}

		if (seq2 != '~')
		{
			input = null;
			return false;
		}

		switch(seq1)
		{
			case '5':
				input = new InputChar(InputChar.EscapeCode.PageUp);
				return true;
			case '6':
				input = new InputChar(InputChar.EscapeCode.PageDown);
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
