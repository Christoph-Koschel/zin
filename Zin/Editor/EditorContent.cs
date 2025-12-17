using System;
using System.Collections.Generic;
using Zin.Editor.Input;

namespace Zin.Editor;

public class EditorContent
{
    private List<string> _rows;
 
    public Cursor ScrollOffset;

    public int Count => _rows.Count;

    public EditorContent(int capacity)
    {
        _rows = new List<string>(capacity);
        ScrollOffset = new Cursor();
    }

    public void OpenContent(string content)
    {
        _rows.Clear();
        _rows.AddRange(content.Split(Environment.NewLine));
    }

    public void OpenContent(IEnumerable<string> lines)
    {
        _rows.Clear();
        _rows.AddRange(lines);
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