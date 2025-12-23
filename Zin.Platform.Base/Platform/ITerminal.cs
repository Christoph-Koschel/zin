using System;
using Zin.Editor.Input;

namespace Zin.Platform;

public interface ITerminal : IDisposable
{
    public int Width { get; }
    public int Height { get; }

    void EnableRawMode();

    void DisableRawMode();

    InputChar Read();

    void Write(string text);
}