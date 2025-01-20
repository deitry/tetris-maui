﻿namespace Tetris.CommonLib;

public class Shape : IShape, IEquatable<Shape>
{
    public const int MaxSize = 4;

    private static bool[,] EmptyGrid => new bool[MaxSize, MaxSize];

    private readonly bool[,] _shape;

    private static readonly char[] AllowedSymbols = ['\n', ' ', Constants.OccupiedCell];

    /// <remarks>
    /// Internal so no one outside cannot define their own shape
    /// </remarks>
    internal Shape(string shapeString)
    {
        var normalized = shapeString.ReplaceLineEndings("\n");

        if (normalized.Any(c => ! AllowedSymbols.Contains(c)))
            throw new ArgumentException("Invalid symbols in shape description");

        var basis = normalized.Split("\n").ToList();
        var maxLength = basis.Max(s => s.Length);

        if (basis.Count > 4 || maxLength > 4)
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

    public int Width => _shape.Width();
    public int Height => _shape.Height();

    public IShape RotatedClockwise
    {
        get
        {
            var newShape = EmptyGrid;
            for (var i = 0; i < MaxSize; i++)
            {
                for (var j = 0; j < MaxSize; j++)
                {
                    newShape[j, i] = _shape[i, j];
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

        for (var i = 0; i < MaxSize; i++)
        for (var j = 0; j < MaxSize; j++)
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
        return HashCode.Combine(_shape, AllowedSymbols);
    }

    public override string ToString()
    {
        return _shape.AsString();
    }
}
