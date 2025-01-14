namespace Tetris.CommonLib;

public interface IGameState
{
    bool CanMove(PositionedShape? currentShape, PositionSpan offset);

    /// <summary>
    /// Merge given shape at its position
    /// </summary>
    void Merge(PositionedShape? currentShape);

    int ClearCompleteRows();

    bool CanSpawn(IShape block);
}
