using System;
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
        editor.Run();

        // Clean up terminal
        Console.Write("\n\rExiting...\n\r");
    }
}
