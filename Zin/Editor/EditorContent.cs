using System;
using System.Collections.Generic;
using System.Linq;

namespace Zin.Editor;

public class EditorContent
{
    private List<string> _rows;

    public Vector2 ScrollOffset;
    public int MaxWidth { get; private set; }

    public int Count => _rows.Count;

    public EditorContent(int capacity)
    {
        _rows = new List<string>(capacity);
        ScrollOffset = new Vector2();
        MaxWidth = 0;
    }

    public void OpenContent(string content)
    {
        _rows.Clear();
        _rows.AddRange(content.Split(Environment.NewLine));
        MaxWidth = _rows.Select(row => row.Length).Max();
    }

    public void OpenContent(IEnumerable<string> lines)
    {
        _rows.Clear();
        _rows.AddRange(lines);
        MaxWidth = _rows.Select(row => row.Length).Max();
    }

    public bool TryGetLine(int i, out string line)
    {
        line = string.Empty;
        if (i < _rows.Count)
        {
            line = _rows[i];
            return true;
        }

        return false;
    }
}