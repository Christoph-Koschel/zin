using System;

namespace Zin.Editor.Input;

public class InputChar : IEquatable<InputChar>
{
    public readonly byte Raw;
    public readonly bool Escape;
    public bool Ctrl => !Escape && Raw is >= 1 and <= 26;
    public bool Invalid => !Escape && Raw == 0;

    public EscapeCode EscCode => (EscapeCode)Raw;

    public InputChar(char c, bool ctrl)
    {
        byte b = (byte)c;
        if (ctrl)
        {
            b = (byte)(b & 0x1F);
        }

        Raw = b;
        Escape = false;
    }

    public InputChar(byte raw)
    {
        Raw = raw;
        Escape = false;
    }

    public InputChar(EscapeCode raw, bool escape)
    {
        Raw = (byte)raw;
        Escape = escape;
    }

    public static bool operator ==(InputChar left, InputChar right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(InputChar left, InputChar right)
    {
        if (left is null)
        {
            return right is not null;
        }

        return !left.Equals(right);
    }

    public bool Equals(InputChar other)
    {
        if (other is null)
        {
            return false;
        }

        return Raw == other.Raw && Escape == other.Escape;
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

        if (obj.GetType() != typeof(InputChar))
        {
            return false;
        }

        return Equals((InputChar)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Raw, Escape);
    }

    public enum EscapeCode : byte
    {
        Escape,

        ArrowUp,
        ArrowDown,
        ArrowLeft,
        ArrowRight,

        PageUp,
        PageDown,
        Delete,
        End,
        Home,

        Enter,
        Backspace
    }
}