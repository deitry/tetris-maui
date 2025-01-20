using System.Diagnostics;

namespace Tetris.CommonLib;

public class RectangleGame2dState : IGame2dState, IEquatable<RectangleGame2dState>
{
    public const char NonOccupiedCell = '-';

    public event Action<bool[,]>? StateUpdated;

    public event Action<int>? RowsCleared;

    public int Width => _grid.GetLength(0);
    public int Height => _grid.GetLength(1);

    public Position SpawnPoint => new((int)(Math.Ceiling(Width / 2f) - 2), TopIndex);

    /// <summary>
    /// Index of most top cell
    /// </summary>
    public int TopIndex => 0;

    public int LeftIndex => 0;

    public int RightIndex => _grid.GetLength(0) - 1;

    /// <summary>
    /// Index of most bottom cell
    /// </summary>
    public int BottomIndex => _grid.GetLength(1) - 1;

    private readonly bool[,] _grid;

    public RectangleGame2dState(int width, int height)
    {
        if (width < 4)
            throw new ArgumentException("Grid is too small!", nameof(width));

        if (height < 4)
            throw new ArgumentException("Grid is too small!", nameof(height));

        _grid = new bool[width, height];
    }

    /// <summary>
    /// Create from visual representation
    /// </summary>
    public RectangleGame2dState(string grid)
    {
        _grid = grid.To2dBoolArray();

        if (_grid.GetLength(0) < 4 || _grid.GetLength(1) < 4)
            throw new ArgumentException("Grid is too small!", nameof(grid));
    }

    public bool CanMove(PositionedShape? currentShape, PositionSpan offset)
    {
        if (currentShape is null)
            return false;

        return CanMerge(currentShape.Shape, currentShape.Position + offset);
    }

    public void Merge(PositionedShape? currentShape)
    {
        if (currentShape is null)
            return;

        var x0 = currentShape.Position.X;
        var y0 = currentShape.Position.Y;

        if (x0 + currentShape.Shape.Width - 1 > RightIndex)
            throw new ArgumentException("Shape is out of bounds", nameof(currentShape));

        if (y0 + currentShape.Shape.Height - 1 > BottomIndex)
            throw new ArgumentException("Shape is out of bounds", nameof(currentShape));

        for (var x = 0; x < currentShape.Shape.Width; x++)
        {
            for (var y = 0; y < currentShape.Shape.Height; y++)
            {
                var shapeCell = currentShape.Shape[x, y];

                var stateX = x0 + x;
                var stateY = y0 + y;

                // minus since distance is measured from top
                if (shapeCell)
                {
                    Debug.Assert(!_grid[stateX, stateY], "cell is already occupied");
                    _grid[stateX, stateY] = true;
                }
            }
        }

        StateUpdated?.Invoke(_grid);
    }

    bool IsRowComplete(int row)
    {
        for (var col = 0; col < Width; col++)
        {
            if (!_grid[col, row])
            {
                return false;
            }
        }

        return true;
    }

    public void ClearCompleteRows()
    {
        var rowsCleared = 0;
        var row = BottomIndex;

        while (row >= TopIndex)
        {
            if (!IsRowComplete(row))
            {
                row--;
                continue;
            }

            RemoveRow(row);

            rowsCleared++;
        }

        if (rowsCleared > 0)
        {
            StateUpdated?.Invoke(_grid.DeepClone());
            RowsCleared?.Invoke(rowsCleared);
        }
    }

    internal void RemoveRow(int row)
    {
        // from bottom to top
        for (var currentRow = row; currentRow >= TopIndex; currentRow--)
        {
            var nextRow = currentRow - 1;
            for (var col = 0; col < Width; col++)
            {
                _grid[col, currentRow] = nextRow >= TopIndex && _grid[col, nextRow];
            }
        }
    }

    public bool CanSpawn(IShape shape) => CanMerge(shape, SpawnPoint);

    public bool CanMerge(IShape shape, Position position)
    {
        var x0 = position.X;
        var y0 = position.Y;

        if (x0 < LeftIndex || x0 + shape.Width - 1 > RightIndex)
            return false;

        if (y0 < TopIndex || y0 + shape.Height - 1 > BottomIndex)
            return false;

        for (var x = 0; x < shape.Width; x++)
        {
            for (var y = 0; y < shape.Height; y++)
            {
                var shapeCell = shape[x, y];
                var stateCell = _grid[x0 + x, y0 + y];

                // minus since distance is measured from top
                if (shapeCell && stateCell)
                    return false;
            }
        }

        return true;
    }

    public bool Equals(RectangleGame2dState? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        for (var i = 0; i < Width; i++)
        {
            for (var j = 0; j < Height; j++)
            {
                if (_grid[i, j] != other._grid[i, j])
                    return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals((RectangleGame2dState)obj);
    }

    public override int GetHashCode()
    {
        return _grid.GetHashCode();
    }

    public override string ToString()
    {
        return _grid.AsString(emptyPlaceholder: '-');
    }
}
