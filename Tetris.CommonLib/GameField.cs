namespace Tetris.CommonLib;

/// <summary>
/// This is where all things happen
/// </summary>
public class GameField : IUserInterfaceHandler
{
    public event Action<int>? RowsCleared;

    public event Action<bool[,]>? StateUpdated;

    public event Action<PositionedShape?>? CurrentShapeUpdated;

    /// <summary>
    /// All accumulated blocks.
    /// </summary>
    /// <remarks>
    /// Does not include currently active shape.
    /// </remarks>
    public readonly IGame2dState CurrentStaticState;

    private PositionedShape? _currentShape;

    public PositionedShape? CurrentShape
    {
        get => _currentShape;
        private set
        {
            _currentShape = value;

            CurrentShapeUpdated?.Invoke(value);
        }
    }

    public GameField(int width, int height)
    {
        CurrentStaticState = new RectangleGame2dState(width, height);
        CurrentStaticState.RowsCleared += rows => RowsCleared?.Invoke(rows);
        CurrentStaticState.StateUpdated += state => StateUpdated?.Invoke(state);
    }

    public void Spawn(IShape shape)
    {
        if (CurrentShape != null)
            throw new Exception("Shape is already spawned");

        CurrentShape = new (shape, Position: CurrentStaticState.SpawnPoint);
        CurrentShape.Updated += s => CurrentShapeUpdated?.Invoke(s);
    }

    public bool CanSpawn(IShape block)
    {
        return CurrentStaticState.CanSpawn(block);
    }

    private bool CanMoveLeft => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Left);

    private bool CanMoveRight => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Right);

    private bool CanMoveDown => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Down);

    private bool CanRotateClockwise => CurrentStaticState.CanMove(CurrentShape?.Rotated, PositionSpan.Zero);

    public void OnMoveLeft()
    {
        if (CurrentShape != null && CanMoveLeft)
        {
            CurrentShape.Position += PositionSpan.Left;

            CurrentShapeUpdated?.Invoke(CurrentShape);
        }
    }

    public void OnMoveRight()
    {
        if (CurrentShape != null && CanMoveRight)
        {
            CurrentShape.Position += PositionSpan.Right;

            CurrentShapeUpdated?.Invoke(CurrentShape);
        }
    }

    public void OnMoveDown()
    {
        if (CurrentShape != null && CanMoveDown)
        {
            CurrentShape.Position += PositionSpan.Down;

            CurrentShapeUpdated?.Invoke(CurrentShape);
        }
    }

    public void OnRotateClockwise()
    {
        if (CurrentShape != null && CanRotateClockwise)
        {
            CurrentShape.RotateClockwise();

            CurrentShapeUpdated?.Invoke(CurrentShape);
        }
    }

    public bool CheckIfMovementIsFinished()
    {
        return !CanMoveLeft && !CanMoveRight && !CanMoveDown && !CanRotateClockwise;
    }

    public void StateBasedActions()
    {
        if (CurrentShape != null)
        {
            if (CanMoveDown)
            {
                OnMoveDown();
            }
            else
            {
                CurrentShape.Updated -= CurrentShapeUpdated;
                CurrentShape = null;
                CurrentStaticState.Merge(CurrentShape);
            }
        }

        CurrentStaticState.ClearCompleteRows();
    }
}
