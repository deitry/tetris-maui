namespace Tetris.CommonLib;

/// <summary>
/// Represents state of game assuming it is 2d
/// </summary>
public interface IGame2dState
{
    int Width { get; }

    int Height { get; }

    event Action<bool[,]> StateUpdated;

    /// <summary>
    /// For score calculation
    /// </summary>
    event Action<int> RowsCleared;

    bool CanMove(PositionedShape? currentShape, PositionSpan offset);

    /// <summary>
    /// Merge given shape at its position
    /// </summary>
    void Merge(PositionedShape? currentShape);

    void ClearCompleteRows();

    Position SpawnPoint { get; }

    bool CanSpawn(IShape block);

    bool[,]? AsArray();
}
