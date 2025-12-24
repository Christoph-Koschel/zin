using System;
using System.Collections.Generic;
using System.Linq;
using Zin.Editor.Common;

namespace Zin.Editor.Buffer;

public class EditorContent
{
    private List<GapBuffer> _rows;

    public int RowCount => _rows.Count;
    public bool OnceModified { get; private set; }

    public EditorContent(int capacity)
    {
        _rows = new List<GapBuffer>(capacity);
        OnceModified = false;
    }

    public void OpenContent(string content)
    {
        _rows.Clear();
        _rows.AddRange(content.Split(Environment.NewLine).Select(line => new GapBuffer(line)));
        OnceModified = false;
    }

    public void OpenContent(IEnumerable<string> lines)
    {
        _rows.Clear();
        _rows.AddRange(lines.Select(line => new GapBuffer(line)));
        OnceModified = false;
    }

    public void Insert(ImmutableVector2 pos, char c) => Insert(pos.X, pos.Y, c);
    public void Insert(Vector2 pos, char c) => Insert(pos.X, pos.Y, c);

    public void Insert(int x, int y, char c)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.InsertAt(x, c);
        OnceModified = true;
    }

    public void Delete(ImmutableVector2 pos) => Delete(pos.X, pos.Y);
    public void Delete(Vector2 pos) => Delete(pos.X, pos.Y);

    public void Delete(int x, int y)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.DeleteAt(x - 1);
        OnceModified = true;
    }

    public void Replace(Vector2 pos, char c) => Replace(pos.X, pos.Y, c);

    public void Replace(int x, int y, char c)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        gapBuffer.ReplaceAt(x, c);
        OnceModified = true;
    }

    public void InsertLineBreak(ImmutableVector2 pos) => InsertLineBreak(pos.X, pos.Y);
    public void InsertLineBreak(Vector2 pos) => InsertLineBreak(pos.X, pos.Y);

    public void InsertLineBreak(int x, int y)
    {
        if (!TryGetLine(y, out GapBuffer gapBuffer))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        GapBuffer nextLine = gapBuffer.SplitAt(x);
        _rows.Insert(y + 1, nextLine);
        OnceModified = true;
    }

    public void RemoveStartLineBreak(ImmutableVector2 pos) => RemoveStartLineBreak(pos.Y);
    public void RemoveStartLineBreak(Vector2 pos) => RemoveStartLineBreak(pos.Y);

    public void RemoveStartLineBreak(int y)
    {
        if (y == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if (!TryGetLine(y, out GapBuffer line))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        if (!TryGetLine(y - 1, out GapBuffer previousLine))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        previousLine.Merge(line);
        _rows.RemoveAt(y);
        OnceModified = true;
    }

    public void RemoveEndLineBreak(ImmutableVector2 pos) => RemoveEndLineBreak(pos.Y);
    public void RemoveEndLineBreak(Vector2 pos) => RemoveEndLineBreak(pos.Y);

    public void RemoveEndLineBreak(int y)
    {
        if (y + 1 >= _rows.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        RemoveStartLineBreak(y + 1);
        OnceModified = true;
    }

    public bool TryGetLine(int y, out GapBuffer gapBuffer)
    {
        gapBuffer = null;
        if (y < 0 || y >= _rows.Count)
        {
            return false;
        }

        gapBuffer = _rows[y];
        return true;

    }
}