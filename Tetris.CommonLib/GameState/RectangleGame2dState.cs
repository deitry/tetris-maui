using System.Diagnostics;

namespace Tetris.CommonLib;

public class RectangleGame2dState : IGame2dState, IEquatable<RectangleGame2dState>
{
    public const char OccupiedCell = '*';
    public const char NonOccupiedCell = '-';

    public event Action<bool[,]>? StateUpdated;

    public event Action<int>? RowsCleared;

    public int Width => _grid.GetLength(0);
    public int Height => _grid.GetLength(1);

    public Position SpawnPoint => new(Width / 2, TopIndex);

    /// <summary>
    /// Index of most top cell
    /// </summary>
    public int TopIndex => _grid.GetLength(1) - 1;

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

        throw new NotImplementedException();
    }

    public void Merge(PositionedShape? currentShape)
    {
        if (currentShape is null)
            return;

        var x0 = currentShape.Position.X;
        var y0 = currentShape.Position.Y;

        for (var x = 0; x < currentShape.Shape.Width; x++)
        {
            for (var y = 0; y < currentShape.Shape.Height; y++)
            {
                var shapeCell = currentShape.Shape[x, y];

                Debug.Assert(!_grid[x0 + x, y0 - y], "cell is already occupied");

                // minus since distance is measured from top
                if (shapeCell)
                {
                    _grid[x0 + x, y0 - y] = true;
                }
            }
        }
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
        var row = 0;
        while (row < Height)
        {
            if (!IsRowComplete(row))
            {
                row++;
                continue;
            }

            RemoveRow(row);

            rowsCleared++;
        }

        StateUpdated?.Invoke(_grid.DeepClone());
        RowsCleared?.Invoke(rowsCleared);
    }

    private void RemoveRow(int row)
    {
        var height = _grid.GetLength(1);
        for (var currentRow = row; currentRow < _grid.Height(); currentRow--)
        {
            for (var col = 0; col < Width; col++)
            {
                _grid[col, currentRow] = currentRow + 1 <= height && _grid[col, currentRow];
            }
        }
    }

    public bool CanSpawn(IShape shape)
    {
        // пытаемся совместить shape с текущим гридом и смотрим что получается
        var x0 = SpawnPoint.X;
        var y0 = SpawnPoint.Y;

        for (var x = 0; x < shape.Width; x++)
        {
            for (var y = 0; y < shape.Height; y++)
            {
                var shapeCell = shape[x, y];
                var stateCell = _grid[x0 + x, y0 - y];

                // minus since distance is measured from top
                if (shapeCell && stateCell)
                {
                    return false;
                }
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
