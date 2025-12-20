using System;

namespace Zin.Editor;

public enum EditorMode
{
    Visuell
}

public static class EditorModeFuncs
{
    public static string GetModeText(this EditorMode mode)
    {
        switch (mode)
        {
            case EditorMode.Visuell:
                return "Visuell";
        }

        throw new ArgumentException("Invalid editor mode");
    }
}
