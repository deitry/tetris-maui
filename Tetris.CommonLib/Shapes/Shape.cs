namespace Tetris.CommonLib;

public class Shape : IShape, IEquatable<Shape>
{
    private static bool[,] EmptyGrid => new bool[Constants.MaxSize, Constants.MaxSize];

    private readonly bool[,] _shape;

    /// <remarks>
    /// Internal so no one outside cannot define their own shape
    /// </remarks>
    internal Shape(string shapeString)
    {
        var normalized = shapeString.ReplaceLineEndings("\n");

        var basis = normalized.Split("\n").ToList();
        var maxLength = basis.Max(s => s.Length);

        if (basis.Count > Constants.MaxSize || maxLength > Constants.MaxSize)
            throw new NotSupportedException("Shape is too big for current implementation");

        _shape = EmptyGrid;
        for (var row = 0; row < basis.Count; row++)
        {
            var line = basis[row];
            for (var col = 0; col < line.Length; col++)
            {
                _shape[col, row] = line[col] == Constants.OccupiedCell;
            }
        }
    }

    private Shape(bool[,] shape)
    {
        _shape = shape;
    }

    public int Width => _shape.OccupiedWidth();
    public int Height => _shape.OccupiedHeight();

    public IShape RotatedClockwise
    {
        get
        {
            var newShape = EmptyGrid;
            for (var row = 0; row < Constants.MaxSize; row++)
            {
                for (var col = 0; col < Constants.MaxSize; col++)
                {
                    newShape[Constants.MaxSize - row - 1, col] = _shape[col, row];
                }
            }

            return new Shape(newShape);
        }
    }

    public bool this[int x, int y] => _shape[x, y];

    public bool Equals(Shape? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        // assuming that shapes are always 4x4

        for (var i = 0; i < Constants.MaxSize; i++)
        for (var j = 0; j < Constants.MaxSize; j++)
        {
            if (_shape[i, j] != other._shape[i, j])
                return false;
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Shape)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_shape);
    }

    public override string ToString()
    {
        return _shape.AsString();
    }
}
