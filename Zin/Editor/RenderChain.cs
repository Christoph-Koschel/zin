using System;
using System.Text;

namespace Zin.Editor;

public sealed class RenderChain
{
    private const string ESC_CLEAR_SCREEN = "\x1b[2J";
    private const string ESC_CLEAR_LINE_RIGHT = "\x1b[K";
    private const string ESC_CLEAR_LINE_LEFT = "\x1b[1K";
    private const string ESC_CLEAR_LINE = "\x1b[2K";

    private const string ESC_CURS_MV_TOP = "\x1b[H";
    private const string ESC_CURS_MV_TO_0 = "\x1b[";
    private const string ESC_CURS_MV_TO_1 = ";";
    private const string ESC_CURS_MV_TO_2 = "H";
    private const string ESC_CURS_HIDDE = "\x1b[?25l";
    private const string ESC_CURS_SHOW = "\x1b[?25h";

    private const string EOL = "\r\n";

    private StringBuilder _renderBuffer;

    public RenderChain(int width, int height)
    {
        _renderBuffer = new StringBuilder(width * height);
    }

    public void PrepareRender() => _renderBuffer.Clear();

    public void Write(string str) => _renderBuffer.Append(str);
    public void Write(string str, int length)
    {
        int computedLength = Math.Min(str.Length, length);
        Span<char> buff = stackalloc char[computedLength];
        str.AsSpan(0, computedLength).CopyTo(buff);
        _renderBuffer.Append(buff);
    }
    public void Write(string str, int offset, int length)
    {
        int computedLength = Math.Min(Math.Max(str.Length - offset, 0), length);
        Span<char> buff = stackalloc char[computedLength];
        str.AsSpan(offset, computedLength).CopyTo(buff);
        _renderBuffer.Append(buff);
    }
    public void Write(char c) => _renderBuffer.Append(c);
    public void Write(int i) => _renderBuffer.Append(i);

    public void LineBreak() => _renderBuffer.Append(EOL);

    public void ClearLineRight() => _renderBuffer.Append(ESC_CLEAR_LINE_RIGHT);
    public void ClearLineLeft() => _renderBuffer.Append(ESC_CLEAR_LINE_LEFT);
    public void ClearLine() => _renderBuffer.Append(ESC_CLEAR_LINE);

    public void ClearScreen() => _renderBuffer.Append(ESC_CLEAR_SCREEN);

    /**
     * Moves the cursor to the given position
     */
    public void MoveCursor(Vector2 cursor)
    {
        _renderBuffer.Append(ESC_CURS_MV_TO_0);
        _renderBuffer.Append(cursor.Y + 1);
        _renderBuffer.Append(ESC_CURS_MV_TO_1);
        _renderBuffer.Append(cursor.X + 1);
        _renderBuffer.Append(ESC_CURS_MV_TO_2);
    }

    /**
     * Moves the cursor to the given position
     */
    public void MoveCursor(int x, int y)
    {
        _renderBuffer.Append(ESC_CURS_MV_TO_0);
        _renderBuffer.Append(y + 1);
        _renderBuffer.Append(ESC_CURS_MV_TO_1);
        _renderBuffer.Append(x + 1);
        _renderBuffer.Append(ESC_CURS_MV_TO_2);
    }

    /**
     * Moves the cursor to the top left corner of the screen.
     */
    public void MoveCursor()
    {
        _renderBuffer.Append(ESC_CURS_MV_TOP);
    }

    public void HideCursor() => _renderBuffer.Append(ESC_CURS_HIDDE);
    public void ShowCursor() => _renderBuffer.Append(ESC_CURS_SHOW);

    public string Render() => _renderBuffer.ToString();
}