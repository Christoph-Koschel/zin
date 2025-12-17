using System.Reflection;
using Zin.Editor.Input;
using Zin.Platform.Base;

namespace Zin.Editor;

public sealed class ZinEditor
{
    private static string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString();
    private static string Title => "Zin Editor";

    private readonly ITerminal _terminal;
    private readonly KeyMap _keyMap;
    private bool _stopped;
    private RenderChain _renderChain;
    
    public Cursor Cursor;
    public EditorContent Content;
    
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
        Content = new EditorContent(terminal.Height);
        Cursor = new Cursor();
    }

    public void Run()
    {
        Render(null);

        while (true)
        {
            InputChar c = _terminal.Read();
            if (c.Invalid)
            {
                continue;
            }

            _keyMap.ExecuteShortcut(this, c);

            if (_stopped)
            {
                break;
            }

            Render(c);
        }
    }

    public void Stop() => _stopped = true;

    private void Render(InputChar c)
    {
        _renderChain.PrepareRender();
        _renderChain.HideCursor();
        _renderChain.MoveCursor();

        RenderRows();

        _renderChain.MoveCursor(Cursor);
        _renderChain.ShowCursor();
        _terminal.Write(_renderChain.Render());
    }

    private void RenderRows()
    {
        for (int y = 0; y < _terminal.Height - 1; y++)
        {
            _renderChain.ClearLine();
            
            if (Content.TryGetLine(y + Content.ScrollOffset.Y, out string line))
            {
                _renderChain.Write(line, Content.ScrollOffset.X, _terminal.Width);
            }
            else if (Content.ScrollOffset.X == 0)
            {
                _renderChain.Write('~');
            }
            _renderChain.LineBreak();
        }

        _renderChain.Write("VISUELL");
        _renderChain.MoveCursor();
    }
}