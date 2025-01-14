namespace Tetris.CommonLib;

/// <summary>
/// This is where all things happen
/// </summary>
public class GameField : IUserInterfaceHandler
{
    public const int Width = 10;

    public const int Height = 20;

    /// <summary>
    /// All accumulated blocks.
    /// </summary>
    /// <remarks>
    /// Does not include currently active shape.
    /// </remarks>
    private readonly IGameState CurrentStaticState = new RectangleGameState(width: 10, height: 20);

    public PositionedShape? CurrentShape { get; private set; }

    public void Spawn(IShape shape)
    {
        if (CurrentShape != null)
            throw new Exception("Shape is already spawned");

        CurrentShape = new (shape, Position: new((int) Math.Ceiling(Width / 2f), Height - 1));
    }

    public bool CanSpawn(IShape block)
    {
        return CurrentStaticState.CanSpawn(block);
    }

    private bool CanMoveLeft => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Left);

    private bool CanMoveRight => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Right);

    private bool CanMoveDown => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Down);

    private bool CanRotateClockwise => false; // TODO

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

    public StateBasedActionsResult StateBasedActions()
    {
        if (CurrentShape != null && CheckIfMovementIsFinished())
        {
            CurrentStaticState.Merge(CurrentShape);
            CurrentShape = null;
        }

        return StateBasedActionsResult.FromRowsCleared(CurrentStaticState.ClearCompleteRows());
    }
}
