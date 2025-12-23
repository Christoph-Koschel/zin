using Zin.Editor.Buffer;
using Zin.Editor.Common;
using Zin.Editor.Mode;

namespace Zin.Editor.Input;

public static class EditorActions
{
    public static void Exit(ZinEditor editor)
    {
        editor.Stop();
    }

    public static void MoveCursorUp(ZinEditor editor)
    {
        ImmutableVector2 cursor = editor.AbsoluteCursor;
        if (cursor.Y > 0)
        {
            editor.SetYCursorAbsolute(cursor.Y - 1);
        }

        if (!editor.Content.TryGetLine(cursor.Y - 1, out GapBuffer gapBuffer))
        {
            return;
        }

        if (cursor.X > gapBuffer.Length)
        {
            editor.SetXCursorAbsolute(gapBuffer.Length);
        }
    }

    public static void MoveCursorDown(ZinEditor editor)
    {
        ImmutableVector2 cursor = editor.AbsoluteCursor;
        if (cursor.Y < editor.Content.RowCount - 1)
        {
            editor.SetYCursorAbsolute(cursor.Y + 1);
        }

        if (!editor.Content.TryGetLine(cursor.Y + 1, out GapBuffer gapBuffer))
        {
            return;
        }

        if (cursor.X > gapBuffer.Length)
        {
            editor.SetXCursorAbsolute(gapBuffer.Length);
        }
    }

    public static void MoveCursorLeft(ZinEditor editor)
    {
        ImmutableVector2 cursor = editor.AbsoluteCursor;
        if (cursor.X > 0)
        {
            editor.SetXCursorAbsolute(cursor.X - 1);
        }
    }

    public static void MoveCursorRight(ZinEditor editor)
    {
        ImmutableVector2 cursor = editor.AbsoluteCursor;
        if (!editor.Content.TryGetLine(cursor.Y, out GapBuffer gapBuffer))
        {
            return;
        }

        if (cursor.X < gapBuffer.Length)
        {
            editor.SetXCursorAbsolute(cursor.X + 1);
        }
    }

    public static void MovePageUp(ZinEditor editor)
    {
        // TODO replace with direct compution
        for (int i = 0; i < editor.ScrollPanelHeight - 1; i++)
        {
            MoveCursorUp(editor);
        }
    }

    public static void MovePageDown(ZinEditor editor)
    {
        // TODO replace with direct compution
        for (int i = 0; i < editor.ScrollPanelHeight - 1; i++)
        {
            MoveCursorDown(editor);
        }
    }

    public static void MoveToStartOfLine(ZinEditor editor)
    {
        editor.SetXCursorAbsolute(0);
    }

    public static void MoveToEndOfLine(ZinEditor editor)
    {
        if (!editor.Content.TryGetLine(editor.AbsoluteCursor.Y, out GapBuffer gapBuffer))
        {
            MoveToStartOfLine(editor);
            return;
        }

        editor.SetXCursorAbsolute(gapBuffer.Length);
    }

    public static void ChangeToInsertMode(ZinEditor editor) => editor.Mode = new InsertMode(editor);
    public static void ChangeToCommandMode(ZinEditor editor) => editor.Mode = new CommandMode(editor);
}