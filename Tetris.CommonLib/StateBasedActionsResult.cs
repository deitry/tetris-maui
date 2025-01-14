namespace Tetris.CommonLib;

/// <summary>
/// Possible outcome after applying all state-based actions
/// </summary>
public class StateBasedActionsResult
{
    public int RowsCleared { get; init; }

    public static StateBasedActionsResult FromRowsCleared(int rowsCleared) =>
        new()
        {
            RowsCleared = rowsCleared,
        };
}
