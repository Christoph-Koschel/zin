using Zin.Editor.Common;
using Zin.Editor.Input;

namespace Zin.Editor.Mode;

public sealed class InsertMode : EditorMode
{
    public override string DisplayName => "Insert";

    public InsertMode(ZinEditor editor) : base(editor)
    {
    }

    public override void HandleInput(InputChar c)
    {
        ImmutableVector2 cursor = Editor.AbsoluteCursor;
        // TODO think about special control chars
        if (c.Ctrl)
        {
            return;
        }

        if (c.Escape)
        {
            HandleEscapeInput(cursor, c);
            return;
        }

        Editor.Content.Insert(cursor, (char)c.Raw);
        Editor.SetXCursorAbsolute(cursor.X + 1);
    }

    private void HandleEscapeInput(ImmutableVector2 cursor, InputChar c)
    {
        if (c.Raw == (byte)InputChar.EscapeCode.Backspace)
        {
            if (cursor.X == 0)
            {
                return;
            }

            Editor.Content.Delete(cursor);
            Editor.SetXCursorAbsolute(cursor.X - 1);
            return;
        }

        if (c.Raw == (byte)InputChar.EscapeCode.Enter)
        {
            Editor.Content.InsertLineBreak(cursor);
            Editor.SetCursorAbsolute(0, cursor.Y + 1);
            return;
        }
    }
}