using System;

namespace Zin.Editor;

public static class EditorActions
{
	public static void Exit(ZinEditor editor)
	{
		editor.Stop();
	}

	public static void MoveCursorUp(ZinEditor editor)
	{
		if (editor.Cursor.Y > 0)
		{
			editor.Cursor.Y--;
		}
	}

	public static void MoveCursorDown(ZinEditor editor)
	{
		if (editor.Cursor.Y < editor.Height)
		{
			editor.Cursor.Y++;
		}
	}

	public static void MoveCursorLeft(ZinEditor editor)
	{
		if (editor.Cursor.X > 0)
		{
			editor.Cursor.X--;
		}
	}

	public static void MoveCursorRight(ZinEditor editor)
	{
		if (editor.Cursor.X < editor.Width)
		{
			editor.Cursor.X++;
		}
	}
}
