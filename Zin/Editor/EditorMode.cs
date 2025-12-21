using System;

namespace Zin.Editor;

[Flags]
public enum EditorMode
{
    Command,
    Insert
}

public static class EditorModeFuncs
{
    public static string GetModeText(this EditorMode mode)
    {
        switch (mode)
        {
            case EditorMode.Command:
                return "Command";
            case EditorMode.Insert:
                return "Insert";
        }

        throw new ArgumentException("Invalid editor mode");
    }
}
