namespace Tetris.CommonLib;

public class RectangleGameState : IGameState
{
    private bool[][] _grid;

    /// <summary>
    /// Rectangle
    /// </summary>
    public RectangleGameState(int width, int height)
    {
        _grid = Enumerable.Repeat(new bool[width], height).ToArray();
    }

    public bool CanMove(PositionedShape? currentShape, PositionSpan offset)
    {
        if (currentShape is null)
            return false;

        throw new NotImplementedException();
    }

    public bool CanMoveDown(PositionedShape? currentShape, int offset)
    {
        if (currentShape is null)
            return false;

        if (offset < 0)
            throw new ArgumentException("Expected non-zero value", nameof(offset));

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
}
