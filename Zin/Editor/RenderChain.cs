using System.Text;

namespace Zin.Editor;

public sealed class RenderChain
{
    private const string ESC_CLEAR_SCREEN = "\x1b[2J";
    private const string ESC_CLEAR_LINE = "\x1b[K";

    private const string ESC_CURS_MV_TOP = "\x1b[H";
    private const string ESC_CURS_MV_TO = "\x1b[{0};{1}H";
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
    public void Write(char c) => _renderBuffer.Append(c);

    public void LineBreak() => _renderBuffer.Append(EOL);

    public void ClearLine() => _renderBuffer.Append(ESC_CLEAR_LINE);

    public void ClearScreen() => _renderBuffer.Append(ESC_CLEAR_SCREEN);

    /**
     * Moves the cursor to the given position
     */
    public void MoveCursor(Cursor cursor)
    {
        _renderBuffer.Append(string.Format(ESC_CURS_MV_TO, cursor.Y + 1, cursor.X + 1));
    }

    /**
     * Moves the cursor to the given position
     */
    public void MoveCursor(int x, int y)
    {
        _renderBuffer.Append(string.Format(ESC_CURS_MV_TO, y + 1, x + 1));
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
