using System.Diagnostics.Contracts;

namespace Tetris.CommonLib;

public interface IShape
{
    int Width { get; }

    int Height { get; }

    [Pure]
    IShape RotatedClockwise { get; }

    public bool this[int x, int y] { get; }
}
