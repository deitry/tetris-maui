using Tetris.CommonLib;

namespace TetrisTests;

public class RectangleGameStateTests
{
    [Test]
    public void Test001_LineCanBeSpawnedInEmptyState()
    {
        const string stateSchema = """
                                   --------
                                   --------
                                   --------
                                   --------
                                   """;
        var state = new RectangleGameState(stateSchema);

        Assert.That(state.CanSpawn(Shapes.Line), Is.True);
    }

    [Test]
    public void Test002_LineCanNotBeSpawnedInFullState()
    {
        const string stateSchema = """
                                   ********
                                   ********
                                   ********
                                   ********
                                   """;
        var state = new RectangleGameState(stateSchema);

        Assert.That(state.CanSpawn(Shapes.Line), Is.False);
    }

    [Test]
    public void Test003_LineIsMergedIntoState()
    {
        const string state1Schema = """
                                    --------
                                    --------
                                    --------
                                    --------
                                    """;

        const string state2Schema = """
                                    --------
                                    --------
                                    --------
                                    ****----
                                    """;

        var state1 = new RectangleGameState(state1Schema);
        var state2 = new RectangleGameState(state2Schema);

        var shape = new PositionedShape(Shapes.Line, new(0, 0));

        state1.Merge(shape);

        Assert.That(state2, Is.EqualTo(state1));
    }

    [Test]
    public void Test004_CanMoveLine()
    {
        const string stateSchema = """
                                   --------
                                   --------
                                   --------
                                   --------
                                   """;

        var state = new RectangleGameState(stateSchema);

        var shape = new PositionedShape(Shapes.Line, new(0, state.Top));

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMove(shape, PositionSpan.Right), Is.True);
            Assert.That(state.CanMove(shape, PositionSpan.Left), Is.False);
            Assert.That(state.CanMove(shape, PositionSpan.Down), Is.True);
        });
    }
}
