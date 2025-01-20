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
    public static readonly PositionSpan Zero = new ();
    public static readonly PositionSpan Left = new (dX: -1);
    public static readonly PositionSpan Right = new (dY: 1);
    public static readonly PositionSpan Down = new (dX: 0, dY: 1);
}

public record PositionedShape(IShape Shape, Position Position)
{
    private Position _position = Position;

    public event Action<PositionedShape>? Updated;

    public void RotateClockwise()
    {
        Shape = Shape.RotatedClockwise;
    }

    public Position Position
    {
        get => _position;
        set
        {
            _position = value;

            Updated?.Invoke(this);
        }
    }

    public IShape Shape { get; private set; } = Shape;
}
