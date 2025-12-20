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
            return;
        }

        if (editor.Cursor.Y == 0 && editor.Content.ScrollOffset.Y > 0)
        {
            editor.Content.ScrollOffset.Y--;
        }
    }

    public static void MoveCursorDown(ZinEditor editor)
    {
        if (editor.Cursor.Y + 1 < editor.ScrollPanelHeight)
        {
            editor.Cursor.Y++;
            return;
        }

        if (editor.Cursor.Y + 1 >= editor.ScrollPanelHeight && editor.Content.Count >= editor.Content.ScrollOffset.Y + 2)
        {
            editor.Content.ScrollOffset.Y++;
        }
    }

    public static void MoveCursorLeft(ZinEditor editor)
    {
        if (editor.Cursor.X > 0)
        {
            editor.Cursor.X--;
            return;
        }

        if (editor.Cursor.X == 0 && editor.Content.ScrollOffset.X > 0)
        {
            editor.Content.ScrollOffset.X--;
        }
    }

    public static void MoveCursorRight(ZinEditor editor)
    {
        if (editor.Cursor.X < editor.Width)
        {
            editor.Cursor.X++;
            return;
        }

        if (editor.Cursor.X == editor.Width && editor.Content.ScrollOffset.X < editor.Content.MaxWidth - 1)
        {
            editor.Content.ScrollOffset.X++;
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
        editor.Cursor.X = 0;
        editor.Content.ScrollOffset.X = 0;
    }

    public static void MoveToEndOfLine(ZinEditor editor)
    {
        if (!editor.Content.TryGetLine(editor.Cursor.Y, out string line))
        {
            MoveToStartOfLine(editor);
            return;
        }

        editor.Cursor.X = Math.Min(line.Length, editor.ScrollPanelWidth + 1);
        editor.Content.ScrollOffset.X = Math.Max(0, line.Length - editor.ScrollPanelWidth);
    }
}