using JetBrains.Annotations;

namespace Tetris.CommonLib;

/// <summary>
/// Collection of available shapes
/// </summary>
[PublicAPI]
public static class Shapes
{
    public static readonly IShape Line = new Shape("****");

    public static readonly IShape Square = new Shape("""
                                                     **
                                                     **
                                                     """);

    public static readonly IShape T = new Shape("""
                                                ***
                                                 *
                                                """);

    public static readonly IShape L = new Shape("""
                                                ***
                                                *
                                                """);

    public static readonly IShape ReverseL = new Shape("""
                                                       ***
                                                         *
                                                       """);

    public static readonly IShape S = new Shape("""
                                                 **
                                                **
                                                """);

    public static readonly IShape Z = new Shape("""
                                                **
                                                 **
                                                """);

    public static readonly IReadOnlyList<IShape> Tetraminoes =
    [
        Line,
        Square,
        T,
        L,
        ReverseL,
        S,
        Z,
    ];
}
