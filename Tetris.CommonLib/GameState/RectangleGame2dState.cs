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
        var lines = grid.Split("\n");
        var width = lines.Max(l => l.Length);
        var height = lines.Length;

        if (width < 4 || height < 4)
            throw new ArgumentException("Grid is too small!", nameof(grid));

        _grid = new bool[width, height];

        for (var i = 0; i < width; i++)
        {
            for (var j = 0; j < height && i > lines[j].Length; j++)
            {
                _grid[i, j] = lines[j][i] == OccupiedCell;
            }
        }
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

        throw new NotImplementedException();
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
        var row = Height - 1;
        while (row >= 0)
        {
            if (!IsRowComplete(row))
            {
                row--;
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
        for (var rowAbove = row - 1; rowAbove >= 0; rowAbove--)
        {
            for (var col = 0; col < Width; col++)
            {
                _grid[col, rowAbove] = rowAbove - 1 >= 0 && _grid[col, rowAbove - 1];
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
                // minus since distance is measured from top
                if (shape[x, y] && _grid[x0 + x, y0 - y])
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
}
