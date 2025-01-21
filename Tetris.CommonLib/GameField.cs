namespace Tetris.CommonLib;

/// <summary>
/// This is where all things happen
/// </summary>
public class GameField : IUserInterfaceHandler
{
    public event Action<int>? RowsCleared;

    public event Action<bool[,]>? StateUpdated;

    public event Action<PositionedShape?>? CurrentShapeUpdated;

    public readonly object _locker = new();

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
        lock (_locker)
        {
            if (!CanSpawn(shape))
                throw new GameOverException();

            if (CurrentShape != null)
                throw new Exception("Shape is already spawned");

            CurrentShape = new (shape, Position: CurrentStaticState.SpawnPoint);
            CurrentShape.Updated += s => CurrentShapeUpdated?.Invoke(s);
        }
    }

    public bool CanSpawn(IShape block)
    {
        lock (_locker)
        {
            return CurrentStaticState.CanSpawn(block);
        }
    }

    private bool CanMoveLeft => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Left);

    private bool CanMoveRight => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Right);

    private bool CanMoveDown => CurrentStaticState.CanMove(CurrentShape, PositionSpan.Down);

    private bool CanRotateClockwise => CurrentStaticState.CanMove(CurrentShape?.Rotated, PositionSpan.Zero);

    public void OnMoveLeft()
    {
        lock (_locker)
        {
            if (CurrentShape != null && CanMoveLeft)
            {
                CurrentShape.Position += PositionSpan.Left;

                CurrentShapeUpdated?.Invoke(CurrentShape);
            }
        }
    }

    public void OnMoveRight()
    {
        lock (_locker)
        {
            if (CurrentShape != null && CanMoveRight)
            {
                CurrentShape.Position += PositionSpan.Right;

                CurrentShapeUpdated?.Invoke(CurrentShape);
            }
        }
    }

    public void OnMoveDown()
    {
        lock (_locker)
        {
            if (CurrentShape != null && CanMoveDown)
            {
                CurrentShape.Position += PositionSpan.Down;

                CurrentShapeUpdated?.Invoke(CurrentShape);
            }
        }
    }

    public void OnRotateClockwise()
    {
        lock (_locker)
        {
            if (CurrentShape != null && CanRotateClockwise)
            {
                CurrentShape.RotateClockwise();

                CurrentShapeUpdated?.Invoke(CurrentShape);
            }
        }
    }

    public bool CheckIfMovementIsFinished()
    {
        lock (_locker)
        {
            return !CanMoveLeft && !CanMoveRight && !CanMoveDown && !CanRotateClockwise;
        }
    }

    public void StateBasedActions()
    {
        lock (_locker)
        {
            if (CurrentShape != null)
            {
                if (CanMoveDown)
                {
                    OnMoveDown();
                }
                else
                {
                    var shape = CurrentShape;
                    CurrentShape.Updated -= CurrentShapeUpdated;
                    CurrentShape = null;
                    CurrentStaticState.Merge(shape);
                }
            }

            CurrentStaticState.ClearCompleteRows();
        }
    }

    public void Clear()
    {
        lock (_locker)
        {
            CurrentShape = null;
            CurrentStaticState.Clear();
        }
    }
}
