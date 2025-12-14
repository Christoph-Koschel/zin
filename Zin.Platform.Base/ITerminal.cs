using System;

namespace Zin.Platform.Base;

public interface ITerminal : IDisposable
{
    public int Width { get; }
    public int Height { get; }

    void EnableRawMode();

    void DisableRawMode();

    InputChar Read();

    void Write(string text);
}