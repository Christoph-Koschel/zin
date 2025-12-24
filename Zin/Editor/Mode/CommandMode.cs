using System.Text;
using Zin.Editor.Common;
using Zin.Editor.Input;

namespace Zin.Editor.Mode;

public sealed class CommandMode : EditorMode
{
    public override string DisplayName => "Command";

    private string _commandResult;
    private StringBuilder _commandText;
    private bool _captureCommand;

    public CommandMode(ZinEditor editor) : base(editor)
    {
        _commandResult = string.Empty;
        _captureCommand = false;
        _commandText = new StringBuilder(16);
    }

    public override bool GetStatusText(out string text)
    {
        text = _captureCommand
            ? _commandText.ToString()
            : _commandResult;

        return true;
    }

    public override void HandleInput(InputChar input)
    {
        if (_captureCommand)
        {
            HandleCapturing(input);
            return;
        }

        if (!input.Ctrl && !input.Escape && input.Raw == ':')
        {
            _commandText.Append(':');
            Editor.ExecuteShortcuts = false;
            Editor.UseVirtualCursor = true;
            Editor.VirutalCursor = new Vector2(1, Editor.Height - 1);
            _captureCommand = true;
            return;
        }
    }

    private void HandleCapturing(InputChar input)
    {
        if (!input.Ctrl && !input.Escape)
        {
            _commandText.Append((char)input.Raw);
            // Is done because when the window resizes, the cursor will be positioned wrong by the Y.
            Editor.VirutalCursor = new Vector2(Editor.VirutalCursor.X + 1, Editor.Height - 1);
            return;
        }

        if (!input.Escape)
        {
            return;
        }

        switch (input.EscCode)
        {
            case InputChar.EscapeCode.Escape:
                _commandText.Clear();
                Editor.ExecuteShortcuts = true;
                Editor.UseVirtualCursor = false;
                _captureCommand = false;
                return;
            case InputChar.EscapeCode.Backspace when _commandText.Length > 1:
                _commandText.Remove(_commandText.Length - 1, 1);
                // Is done because when the window resizes, the cursor will be positioned wrong by the Y.
                Editor.VirutalCursor = new Vector2(Editor.VirutalCursor.X - 1, Editor.Height - 1);
                return;
            case InputChar.EscapeCode.Enter:
                _commandResult = Editor.CommandHandler.Execute(Editor, _commandText.ToString());
                _commandText.Clear();
                Editor.ExecuteShortcuts = true;
                Editor.UseVirtualCursor = false;
                _captureCommand = false;
                return;
        }
    }
}