namespace Tetris.CommonLib;

public record Position(int X, int Y)
{
    public static Position operator+(Position p, PositionSpan delta)
    {
        return new (X: p.X + delta.dX, Y: p.Y + delta.dY);
    }
}

/// <summary>
/// Represents delta of two Positions.
/// Static members are less possible quantum of movement.
/// </summary>
public record PositionSpan(int dX = 0, int dY = 0)
{
    public static readonly PositionSpan Zero = new (0, 0);
    public static readonly PositionSpan Left = new (-1, 0);
    public static readonly PositionSpan Right = new (1, 0);
    public static readonly PositionSpan Down = new (0, -1);
}

public record PositionedShape(IShape Shape, Position Position)
{
    public void RotateClockwise()
    {
        Shape = Shape.RotatedClockwise;
    }

    public Position Position { get; set; } = Position;
    public IShape Shape { get; private set; } = Shape;
}
