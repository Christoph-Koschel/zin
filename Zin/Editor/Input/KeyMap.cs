using Zin.Platform.Base;

namespace Zin.Editor.Input;

public sealed class KeyMap
{
    public InputChar ExitShortCut = new InputChar('q', true);
	public InputChar MoveCursorUp = new InputChar(InputChar.EscapeCode.ArrowUp, true);
	public InputChar MoveCursorLeft = new InputChar(InputChar.EscapeCode.ArrowLeft, true);
	public InputChar MoveCursorDown = new InputChar(InputChar.EscapeCode.ArrowDown, true);
	public InputChar MoveCursorRight = new InputChar(InputChar.EscapeCode.ArrowRight, true);

    public KeyMap()
    {

    }

    public void RunKeyShortcut(ZinEditor editor, InputChar input)
    {
        if (input == ExitShortCut)
        {
            editor.Stop();
            return;
        }
		
		if (input == MoveCursorUp)
		{
			if (editor.Cursor.Y >= 0)
			{			
				editor.Cursor.Y--;
			}
			return;
		}
		
		if (input == MoveCursorLeft)
		{
			if (editor.Cursor.X >= 0)
			{
				editor.Cursor.X--;
			}
			return;
		}
		
		if (input == MoveCursorDown)
		{
			if (editor.Cursor.Y < editor.Height)
			{
				editor.Cursor.Y++;
			}
			return;
		}
		
		if (input == MoveCursorRight)
		{
			if (editor.Cursor.X < editor.Width)
			{
				editor.Cursor.X++;
			}
			return;
		}
    }
}
