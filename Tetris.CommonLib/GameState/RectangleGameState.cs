namespace Tetris.CommonLib;

public class RectangleGameState : IGameState, IEquatable<RectangleGameState>
{
    public int Top => _grid.Length - 1;

    private readonly bool[][] _grid;

    public RectangleGameState(int width, int height)
    {
        _grid = Enumerable.Repeat(new bool[width], height).ToArray();
    }

    /// <summary>
    /// Create from visual representation
    /// </summary>
    public RectangleGameState(string grid)
    {
        throw new NotImplementedException();
    }

    public bool CanMove(PositionedShape? currentShape, PositionSpan offset)
    {
        if (currentShape is null)
            return false;

        throw new NotImplementedException();
    }

    public void Merge(PositionedShape? currentShape)
    {
        if (currentShape is null)
            return;

        throw new NotImplementedException();
    }

    public int ClearCompleteRows()
    {
        throw new NotImplementedException();
    }

    public bool CanSpawn(IShape shape)
    {
        throw new NotImplementedException();
    }

    public bool Equals(RectangleGameState? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((RectangleGameState)obj);
    }

    public override int GetHashCode()
    {
        return _grid.GetHashCode();
    }
}
