using System;

namespace Zin.Editor.Input;

public sealed class Shortcut : IEquatable<Shortcut>
{
    public readonly Type EditorModeType;
    public readonly InputChar[] Sequence;

    private readonly int _hashCode;

    public Shortcut(InputChar[] sequence)
    {
        EditorModeType = null;
        Sequence = sequence;

        HashCode hash = new HashCode();
        foreach (InputChar key in sequence)
        {
            hash.Add(key);
        }

        _hashCode = hash.ToHashCode();
    }

    public Shortcut(InputChar key)
    {
        EditorModeType = null;
        Sequence = [key];

        _hashCode = key.GetHashCode();
    }

    public Shortcut(Type editorModeType, InputChar[] sequence)
    {
        EditorModeType = editorModeType;
        Sequence = sequence;

        HashCode hash = new HashCode();
        hash.Add(EditorModeType);
        foreach (InputChar key in sequence)
        {
            hash.Add(key);
        }

        _hashCode = hash.ToHashCode();
    }

    public Shortcut(Type editorModeType, InputChar key)
    {
        EditorModeType = editorModeType;
        Sequence = [key];

        HashCode hash = new HashCode();
        hash.Add(EditorModeType);
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

        return EditorModeType == other.EditorModeType && Sequence.SequenceEqual(other.Sequence);
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