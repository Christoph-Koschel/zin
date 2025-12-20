using System;
using Zin.Platform.Base;

namespace Zin.Platform.Windows.Managed;

public sealed class WindowsTerminal : ITerminal
{
    public int Width => Console.WindowWidth;
    public int Height => Console.WindowHeight;

    public void EnableRawMode()
    {
        if (TermShim.EnterRawMode() != 1)
        {
            throw new Exception("Fail to enable raw mode");
        }
    }

    public void DisableRawMode()
    {
        if (TermShim.ExitRawMode() != 1)
        {
            throw new Exception("Fail to disable raw mode");
        }
    }

    public InputChar Read()
    {
        if (TermShim.Read(out byte c) != 1)
        {
            return new InputChar(0);
        }

        return new InputChar(c);
    }

    public void Write(string text) => TermShim.Write(text, text.Length);

    public void Dispose()
    {
        DisableRawMode();
    }
}