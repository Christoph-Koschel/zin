using System;
using Zin.Editor.Buffer;
using Zin.Editor.Common;
using Zin.Editor.Input;
using Zin.Editor.Mode;
using Zin.Editor.Rendering;
using Zin.Platform;

namespace Zin.Editor;

public sealed class ZinEditor
{
    private readonly ITerminal _terminal;
    private readonly KeyMap _keyMap;
    private bool _stopped;
    private RenderChain _renderChain;
    private bool _ignoreDirty;
    private Vector2 _cursor;
    private Vector2 _offset;

    public EditorContent Content;
    public EditorMode Mode;

    public ImmutableVector2 AbsoluteCursor => new ImmutableVector2(_cursor.X + _offset.X, _cursor.Y + _offset.Y);
    public int Width => _terminal.Width;
    public int Height => _terminal.Height;

    public int ScrollPanelWidth => Width;
    public int ScrollPanelHeight => Height - 1;

    public ZinEditor(ITerminal terminal, KeyMap keyMap)
    {
        _terminal = terminal;
        _keyMap = keyMap;
        _stopped = false;
        _renderChain = new RenderChain(terminal.Width, terminal.Height);
        _ignoreDirty = true;
        _cursor = new Vector2();
        _offset = new Vector2();

        Content = new EditorContent(terminal.Height);
        Mode = new CommandMode(this);
    }

    public void Run()
    {
        Render();

        while (!_stopped)
        {
            InputChar c = _terminal.Read();
            if (c.Invalid)
            {
                Render();
                continue;
            }

            if (_keyMap.ExecuteShortcut(this, c))
            {
                Render();
                continue;
            }

            Mode.HandleInput(c);

            Render();
        }
    }

    public void Stop() => _stopped = true;

    public void SetCursorAbsolute(Vector2 pos) => SetCursorAbsolute(pos.X, pos.Y);

    public void SetCursorAbsolute(int x, int y)
    {
        SetXCursorAbsolute(x);
        SetYCursorAbsolute(y);
        // ignoreDirty is set to true through
        // SetXCursorAbsolute and SetYCursorAbsolute
    }

    public void SetXCursorAbsolute(int x)
    {
        if (x < _offset.X)
        {
            _offset.X = x;
            _ignoreDirty = true;
        } else if (x >= _offset.X + ScrollPanelWidth)
        {
            _offset.X = x - ScrollPanelWidth + 1;
            _ignoreDirty = true;
        }

        _cursor.X = x - _offset.X;
    }

    public void SetYCursorAbsolute(int y)
    {
        if (y < _offset.Y)
        {
            _offset.Y = y;
            _ignoreDirty = true;
        } else if (y >= _offset.Y + ScrollPanelHeight)
        {
            _offset.Y = y - ScrollPanelHeight + 1;
            _ignoreDirty = true;
        }

        _cursor.Y = y - _offset.Y;
    }

    private void Render()
    {
        _renderChain.PrepareRender();
        _renderChain.HideCursor();
        _renderChain.MoveCursor();

        RenderRows();
        RenderBottomLine();

        _renderChain.MoveCursor(_cursor);
        _renderChain.ShowCursor();
        _terminal.Write(_renderChain.Render());
    }


    private void RenderRows()
    {
        for (int y = 0; y < _terminal.Height - 1; y++)
        {
            if (Content.TryGetLine(y + _offset.Y, out GapBuffer line))
            {
                if (_ignoreDirty || line.Dirty)
                {
                    _renderChain.ClearLineRight();
                    _renderChain.Write(line.ToString() , _offset.X, _terminal.Width);
                    line.Dirty = false;
                }
            }
            else if (_offset.X == 0)
            {
                _renderChain.ClearLineRight();
                _renderChain.Write('~');
            }
            _renderChain.LineBreak();
        }

        _ignoreDirty = false;
    }

    private void RenderBottomLine()
    {
        string modeText = Mode.DisplayName;
        string xText = Convert.ToString(_cursor.X + _offset.X + 1);
        string yText = Convert.ToString(_cursor.Y + _offset.Y + 1);
        _renderChain.MoveCursor(Width - xText.Length - yText.Length - modeText.Length - 2, Height - 1);
        _renderChain.ClearLine();
        _renderChain.Write(modeText);
        _renderChain.Write(' ');
        _renderChain.Write(xText);
        _renderChain.Write(':');
        _renderChain.Write(yText);
    }
}