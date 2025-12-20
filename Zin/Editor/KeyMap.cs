using System;
using System.Collections.Generic;
using Zin.Platform.Base;

namespace Zin.Editor;

public sealed class KeyMap
{
    private readonly Dictionary<InputChar, Action<ZinEditor>> _actions;

    public KeyMap()
    {
        _actions = new Dictionary<InputChar, Action<ZinEditor>>();
    }

    public void RegisterAction(InputChar shortcut, Action<ZinEditor> action)
    {
        _actions[shortcut] = action;
    }

    public void ExecuteShortcut(ZinEditor editor, InputChar input)
    {
        if (_actions.TryGetValue(input, out Action<ZinEditor> action))
        {
            action.Invoke(editor);
        }
    }

    public static KeyMap Default()
    {
        KeyMap map = new KeyMap();
        map.RegisterAction(new InputChar('q', true), EditorActions.Exit);

        map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowUp, true), EditorActions.MoveCursorUp);
        map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowDown, true), EditorActions.MoveCursorDown);
        map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowLeft, true), EditorActions.MoveCursorLeft);
        map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowRight, true), EditorActions.MoveCursorRight);

        // map.RegisterAction(new InputChar('w', false), EditorActions.MoveCursorUp);
        // map.RegisterAction(new InputChar('s', false), EditorActions.MoveCursorDown);
        // map.RegisterAction(new InputChar('a', false), EditorActions.MoveCursorLeft);
        // map.RegisterAction(new InputChar('d', false), EditorActions.MoveCursorRight);

        map.RegisterAction(new InputChar(InputChar.EscapeCode.PageUp, true), EditorActions.MovePageUp);
        map.RegisterAction(new InputChar(InputChar.EscapeCode.PageDown, true), EditorActions.MovePageDown);

        map.RegisterAction(new InputChar(InputChar.EscapeCode.Home, true), EditorActions.MoveToStartOfLine);
        map.RegisterAction(new InputChar(InputChar.EscapeCode.End, true), EditorActions.MoveToEndOfLine);

        return map;
    }
}