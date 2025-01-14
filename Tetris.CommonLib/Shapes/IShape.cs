using System.Diagnostics.Contracts;

namespace Tetris.CommonLib;

public interface IShape
{
    [Pure]
    IShape RotatedClockwise { get; }
}
