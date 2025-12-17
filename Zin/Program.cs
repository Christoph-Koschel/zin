using System;
using System.Collections.Generic;
using Zin.Editor;
using Zin.Editor.Input;
using Zin.Platform.Base;

namespace Zin;

public static class Program
{
    public static void Main(string[] args)
    {
#if WINDOWS_X64
        using ITerminal terminal = new Platform.Windows.Managed.WindowsTerminal();
#elif LINUX_X64
        using ITerminal terminal = new Platform.Linux.Managed.LinuxTerminal();
#else
#error Unknown target platform
#endif
        KeyMap keyMap = KeyMap.Default();
        terminal.EnableRawMode();

        ZinEditor editor = new ZinEditor(terminal, keyMap);

        editor.Content.OpenContent(GenerateLines());

        editor.Run();
    }

    private static IEnumerable<string> GenerateLines()
    {
        for (int i = 0; i < 200; i++)
        {
            yield return  i + "] Hello this is some line";
        }
    }
}