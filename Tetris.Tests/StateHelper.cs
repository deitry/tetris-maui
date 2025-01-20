using Tetris.CommonLib;

namespace TetrisTests;

public static class StateManager
{
    public static readonly RectangleGame2dState Empty9x4 = new ("""
                                                                --------
                                                                --------
                                                                --------
                                                                --------
                                                                """);

    public static readonly RectangleGame2dState Full9x4 = new ("""
                                                               ********
                                                               ********
                                                               ********
                                                               ********
                                                               """);
}
