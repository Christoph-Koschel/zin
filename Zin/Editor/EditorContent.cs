using System;
using System.Collections.Generic;
using System.Linq;

namespace Zin.Editor;

public class EditorContent
{
    private List<GapBuffer> _rows;

    public int RowCount => _rows.Count;

    public EditorContent(int capacity)
    {
        _rows = new List<GapBuffer>(capacity);
    }

    public void OpenContent(string content)
    {
        _rows.Clear();
        _rows.AddRange(content.Split(Environment.NewLine).Select(line => new GapBuffer(line)));
    }

    public void OpenContent(IEnumerable<string> lines)
    {
        _rows.Clear();
        _rows.AddRange(lines.Select(line => new GapBuffer(line)));
    }

    public void Insert(Vector2 pos, char c) => Insert(pos.X, pos.Y, c);

    public void Insert(int x, int y, char c)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.InsertAt(x, c);
    }

    public void Delete(Vector2 pos) => Delete(pos.X, pos.Y);

    public void Delete(int x, int y)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.DeleteAt(x);
    }

    public void Replace(Vector2 pos, char c) => Replace(pos.X, pos.Y, c);

    public void Replace(int x, int y, char c)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.ReplaceAt(x, c);
    }

    public bool TryGetLine(int i, out GapBuffer gapBuffer)
    {
        gapBuffer = null;
        if (i < 0 || i >= _rows.Count)
        {
            return false;
        }

        gapBuffer = _rows[i];
        return true;

    }
}