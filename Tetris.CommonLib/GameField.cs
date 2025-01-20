﻿namespace Tetris.CommonLib;

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
        CurrentStaticState.RowsCleared += rows => RowsCleared?.Invoke(rows);
        CurrentStaticState.StateUpdated += state => StateUpdated?.Invoke(state);
    }

    public void Spawn(IShape shape)
    {
        if (CurrentShape != null)
            throw new Exception("Shape is already spawned");

        CurrentShape = new (shape, Position: CurrentStaticState.SpawnPoint);
        CurrentShape.Updated += s => CurrentShapeMoved?.Invoke(s);
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

            CurrentShapeMoved?.Invoke(CurrentShape);
        }
    }

    public void OnMoveRight()
    {
        if (CurrentShape != null && CanMoveRight)
        {
            CurrentShape.Position += PositionSpan.Right;

            CurrentShapeMoved?.Invoke(CurrentShape);
        }
    }

    public void OnMoveDown()
    {
        if (CurrentShape != null && CanMoveDown)
        {
            CurrentShape.Position += PositionSpan.Down;

            CurrentShapeMoved?.Invoke(CurrentShape);
        }
    }

    public void OnRotateClockwise()
    {
        if (CurrentShape != null && CanRotateClockwise)
        {
            CurrentShape.RotateClockwise();

            CurrentShapeMoved?.Invoke(CurrentShape);
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
                CurrentStaticState.Merge(CurrentShape);
                CurrentShape.Updated -= CurrentShapeMoved;
                CurrentShape = null;
            }
        }

        CurrentStaticState.ClearCompleteRows();
    }
}
