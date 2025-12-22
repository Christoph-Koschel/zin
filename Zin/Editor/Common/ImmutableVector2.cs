namespace Zin.Editor.Common;

public struct ImmutableVector2
{
    public readonly int X;
    public readonly int Y;

    public ImmutableVector2(Vector2 vector)
    {
        X = vector.X;
        Y = vector.Y;
    }

    public ImmutableVector2(int x, int y)
    {
        X = x;
        Y = y;
    }
}