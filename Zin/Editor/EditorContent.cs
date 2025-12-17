using System;
using System.Collections.Generic;
using System.Linq;
using Zin.Editor.Input;

namespace Zin.Editor;

public class EditorContent
{
    private List<string> _rows;
 
    public Cursor ScrollOffset;
    public int MaxWidth { get; private set; }

    public int Count => _rows.Count;

    public EditorContent(int capacity)
    {
        _rows = new List<string>(capacity);
        ScrollOffset = new Cursor();
        MaxWidth = 0;
    }

    public void OpenContent(string content)
    {
        _rows.Clear();
        _rows.AddRange(content.Split(Environment.NewLine));
        _rows.Select(row => row.Length).Max();
    }

    public void OpenContent(IEnumerable<string> lines)
    {
        _rows.Clear();
        _rows.AddRange(lines);
        _rows.Select(row => row.Length).Max();
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