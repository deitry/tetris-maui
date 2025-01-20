namespace Tetris.CommonLib;

/// <summary>
/// This is where all things happen
/// </summary>
public class GameField : IUserInterfaceHandler
{
    public event Action<int>? RowsCleared;

    public event Action<bool[,]>? StateUpdated;

    public event Action<PositionedShape>? CurrentShapeMoved;

    /// <summary>
    /// All accumulated blocks.
    /// </summary>
    /// <remarks>
    /// Does not include currently active shape.
    /// </remarks>
    public readonly IGame2dState CurrentStaticState;

    public PositionedShape? CurrentShape { get; private set; }

    public GameField(int width, int height)
    {
        CurrentStaticState = new RectangleGame2dState(width, height);
        CurrentStaticState.RowsCleared += RowsCleared;
        CurrentStaticState.StateUpdated += StateUpdated;
    }

    public void Spawn(IShape shape)
    {
        if (CurrentShape != null)
            throw new Exception("Shape is already spawned");

        CurrentShape = new (shape, Position: new((int) Math.Ceiling(CurrentStaticState.Width / 2f), CurrentStaticState.Height - 1));
        CurrentShape.Updated += CurrentShapeMoved;
    }

    public bool CanSpawn(IShape block)
    {
        return CurrentStaticState.CanSpawn(block);
    }

    private bool CanMoveLeft => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Left);

    private bool CanMoveRight => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Right);

    private bool CanMoveDown => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Down);

    private bool CanRotateClockwise => throw new NotImplementedException();

    public void OnMoveLeft()
    {
        if (CurrentShape != null && CanMoveLeft)
        {
            CurrentShape.Position += PositionSpan.Left;
        }
    }

    public void OnMoveRight()
    {
        if (CurrentShape != null && CanMoveRight)
        {
            CurrentShape.Position += PositionSpan.Right;
        }
    }

    public void OnMoveDown()
    {
        if (CurrentShape != null && CanMoveDown)
        {
            CurrentShape.Position += PositionSpan.Down;
        }
    }

    public void OnRotateClockwise()
    {
        if (CurrentShape != null && CanRotateClockwise)
        {
            CurrentShape.RotateClockwise();
        }
    }

    public bool CheckIfMovementIsFinished()
    {
        return !CanMoveLeft && !CanMoveRight && !CanMoveDown && !CanRotateClockwise;
    }

    public void StateBasedActions()
    {
        if (CurrentShape != null && CheckIfMovementIsFinished())
        {
            CurrentStaticState.Merge(CurrentShape);
            CurrentShape.Updated -= CurrentShapeMoved;
            CurrentShape = null;
        }

        CurrentStaticState.ClearCompleteRows();
    }
}
