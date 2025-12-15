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
	public int Width => _terminal.Width;
	public int Height => _terminal.Height;

    public ZinEditor(ITerminal terminal, KeyMap keyMap)
    {
        _terminal = terminal;
        _keyMap = keyMap;
        _stopped = false;
        _renderChain = new RenderChain(terminal.Width, terminal.Height);
        Cursor = new Cursor();
		Cursor.Y = 10;
    }

    public void Run()
    {
        Render();

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
                _renderChain.PrepareRender();
                _renderChain.ClearScreen();
                _renderChain.MoveCursor();
                _terminal.Write(_renderChain.Render());
                break;
            }
            
			Render();
        }
    }

    public void Stop() => _stopped = true;

    private void Render()
    {
        _renderChain.PrepareRender();
        _renderChain.HideCursor();
		_renderChain.MoveCursor();

        RenderRows();

        string title = $"{Title} v{Version}";
        _renderChain.MoveCursor(Cursor);
        _renderChain.Write(title);

        _renderChain.MoveCursor(Cursor);
        _renderChain.ShowCursor();
        _terminal.Write(_renderChain.Render());
    }

    private void RenderRows()
    {
        for (int y = 0; y < _terminal.Height - 1; y++)
        {
            _renderChain.Write('~');
            _renderChain.ClearLine();
            _renderChain.LineBreak();
        }

        _renderChain.Write("~");
        _renderChain.MoveCursor();
    }
}
