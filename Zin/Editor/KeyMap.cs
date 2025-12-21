using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Zin.Platform.Base;

namespace Zin.Editor;

public sealed class KeyMap
{
    public static long TimeoutMs = 500;

    private readonly Dictionary<Shortcut, Action<ZinEditor>> _actions;
    private readonly List<InputChar> _buffer;
    private long _lastKeyTimeMs;

    public KeyMap()
    {
        _actions = new Dictionary<Shortcut, Action<ZinEditor>>();
        _buffer = new List<InputChar>(5);
        _lastKeyTimeMs = 0;
    }

    public void RegisterAction(Shortcut shortcut, Action<ZinEditor> action)
    {
        _actions[shortcut] = action;
    }

    public void ExecuteShortcut(ZinEditor editor, InputChar input)
    {
        long now = Stopwatch.GetTimestamp();
        long elapsedMs = Stopwatch.GetElapsedTime(_lastKeyTimeMs, now).Milliseconds;

        if (_buffer.Count > 0 && elapsedMs > TimeoutMs)
        {
            TryExecuteExact(editor);
            _buffer.Clear();
        }

        _buffer.Add(input);
        _lastKeyTimeMs = now;

        bool hasLonger = false;
        Action<ZinEditor> exactMatch = null;

        foreach ((Shortcut shortcut, Action<ZinEditor> action) in _actions)
        {
            if (!IsPrefix(shortcut))
            {
                continue;
            }

            if (shortcut.Sequence.Length == _buffer.Count)
            {
                exactMatch = action;
            }
            else if (shortcut.Sequence.Length > _buffer.Count)
            {
                hasLonger = true;
            }
        }

        if (exactMatch is not null && !hasLonger)
        {
            exactMatch.Invoke(editor);
            _buffer.Clear();
        }
        else if (exactMatch is null && !hasLonger)
        {
            _buffer.Clear();
        }
    }

    private bool IsPrefix(Shortcut shortcut)
    {
        if (_buffer.Count > shortcut.Sequence.Length)
        {
            return false;
        }

        for (int i = 0; i < _buffer.Count; i++)
        {
            if (!shortcut.Sequence[i].Equals(_buffer[i]))
            {
                return false;
            }
        }

        return true;
    }

    private void TryExecuteExact(ZinEditor editor)
    {
        KeyValuePair<Shortcut, Action<ZinEditor>> exact = _actions.FirstOrDefault(s =>
            s.Key.Sequence.Length == _buffer.Count &&
            s.Key.Sequence.SequenceEqual(_buffer));

        exact.Value?.Invoke(editor);
    }

    public static KeyMap Default()
    {
        KeyMap map = new KeyMap();
        map.RegisterAction(new Shortcut(EditorMode.Command, [new InputChar('q', true), new InputChar('q', true)]), EditorActions.Exit);

        // map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowUp, true), EditorActions.MoveCursorUp);
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowDown, true), EditorActions.MoveCursorDown);
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowLeft, true), EditorActions.MoveCursorLeft);
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.ArrowRight, true), EditorActions.MoveCursorRight);

        // map.RegisterAction(new InputChar('w', false), EditorActions.MoveCursorUp);
        // map.RegisterAction(new InputChar('s', false), EditorActions.MoveCursorDown);
        // map.RegisterAction(new InputChar('a', false), EditorActions.MoveToStartOfLine);
        // map.RegisterAction(new InputChar('d', false), EditorActions.MoveToEndOfLine);
        //
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.PageUp, true), EditorActions.MovePageUp);
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.PageDown, true), EditorActions.MovePageDown);
        //
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.Home, true), EditorActions.MoveToStartOfLine);
        // map.RegisterAction(new InputChar(InputChar.EscapeCode.End, true), EditorActions.MoveToEndOfLine);

        return map;
    }
}