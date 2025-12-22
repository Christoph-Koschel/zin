using System;

namespace Zin.Editor.Buffer;

public sealed class GapBuffer
{
    private char[] _buffer;
    private int _gapStart;
    private int _gapEnd;

    public int Length => _buffer.Length - (_gapEnd - _gapStart);
    public bool Dirty;

    public GapBuffer(string text)
    {
        _buffer = new char[Math.Max(32, text.Length * 2)];
        Array.Copy(text.ToCharArray(), _buffer, text.Length);
        _gapStart = text.Length;
        _gapEnd = _buffer.Length;
        Dirty = true;
    }

    public void InsertAt(int index, char c)
    {
        MoveGap(index);
        EnsureGap(1);
        _buffer[_gapStart++] = c;
        Dirty = true;
    }

    public void DeleteAt(int index)
    {
        if (index < 0 || index >= Length)
        {
            return;
        }

        MoveGap(index);
        _gapStart--;
        Dirty = true;
    }

    public void ReplaceAt(int index, char c)
    {
        if (index < 0 || index >= Length)
        {
            return;
        }

        MoveGap(index + 1);
        _buffer[_gapStart - 1] = c;
        Dirty = true;
    }

    public char CharAt(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        return index < _gapStart
            ? _buffer[index]
            : _buffer[index + (_gapEnd - _gapStart)];
    }

    private void MoveGap(int index)
    {
        index = Math.Clamp(index, 0, Length);

        while (_gapStart < index)
        {
            _buffer[_gapStart] = _buffer[_gapEnd];
            _gapStart++;
            _gapEnd--;
        }

        while (_gapEnd > index)
        {
            _gapEnd--;
            _gapStart++;
            _buffer[_gapEnd] = _buffer[_gapStart];
        }
    }

    private void EnsureGap(int size)
    {
        if (_gapEnd - _gapStart >= size)
        {
            return;
        }

        char[] buffer = new char[_buffer.Length * 2];
        int gapEnd = buffer.Length - (_buffer.Length - _gapEnd);

        Array.Copy(_buffer, 0, buffer, 0, _gapStart);
        Array.Copy(_buffer, _gapEnd, buffer, gapEnd, _buffer.Length - _gapEnd);

        _gapEnd = gapEnd;
        _buffer = buffer;
    }

    public override string ToString() =>
        new string(_buffer, 0, _gapStart) +
        new string(_buffer, _gapEnd, _buffer.Length - _gapEnd);
}