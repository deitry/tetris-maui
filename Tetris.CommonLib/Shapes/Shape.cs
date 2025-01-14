namespace Tetris.CommonLib;

public class Shape : IShape, IEquatable<Shape>
{
    private readonly bool[][] _shape;

    private static readonly char[] AllowedSymbols = ['\n', ' ', '*'];

    /// <remarks>
    /// Internal so no one outside cannot define their own shape
    /// </remarks>
    internal Shape(string shape)
    {
        var normalized = shape.ReplaceLineEndings("\n");

        if (normalized.Any(c => ! AllowedSymbols.Contains(c)))
            throw new ArgumentException("Invalid symbols in shape description");

        var basis = normalized.Split("\n").ToList();

        if (basis.Count > 4 || basis.Any(s => s.Length > 4))
            throw new NotSupportedException("Shape is too big for current implementation");

        _shape = new bool[basis.Count][];

        for (var i = 0; i < basis.Count; i++)
        {
            var line = basis[i];
            _shape[i] = new bool[line.Length];

            for (var j = 0; j < line.Length; j++)
            {
                _shape[i][j] = line[j] == '*';
            }
        }
    }

    public IShape RotatedClockwise => throw new NotImplementedException();

    public bool Equals(Shape? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        if (_shape.Length != other._shape.Length)
            return false;

        for (var i = 0; i < _shape.Length; i++)
        {
            if (! _shape[i].SequenceEqual(other._shape[i]))
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
        return HashCode.Combine(_shape, AllowedSymbols);
    }
}
