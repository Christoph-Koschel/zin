using System;
using Zin.Platform.Base;

namespace Zin.Editor.Input;

public sealed class Shortcut : IEquatable<Shortcut>
{
    public readonly EditorMode Mode;
    public readonly InputChar[] Sequence;

    private readonly int _hashCode;

    public Shortcut(EditorMode mode, InputChar[] sequence)
    {
        Mode = mode;
        Sequence = sequence;

        HashCode hash = new HashCode();
        hash.Add(Mode);
        foreach (InputChar key in sequence)
        {
            hash.Add(key);
        }

        _hashCode = hash.ToHashCode();
    }

    public Shortcut(EditorMode mode, InputChar key)
    {
        Mode = mode;
        Sequence = [key];

        HashCode hash = new HashCode();
        hash.Add(Mode);
        hash.Add(key);

        _hashCode = hash.ToHashCode();
    }

    public static bool operator ==(Shortcut left, Shortcut right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(Shortcut left, Shortcut right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return !left.Equals(right);
    }

    public bool Equals(Shortcut other)
    {
        if (other is null)
        {
            return false;
        }

        return Mode == other.Mode && Sequence.SequenceEqual(other.Sequence);
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof(Shortcut))
        {
            return false;
        }

        return Equals((Shortcut)obj);
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }
}