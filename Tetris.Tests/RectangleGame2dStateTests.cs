using Tetris.CommonLib;

namespace TetrisTests;

public class RectangleGame2dStateTests
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
        var state = new RectangleGame2dState(stateSchema);

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
        var state = new RectangleGame2dState(stateSchema);

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

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = new RectangleGame2dState(state2Schema);

        var shape = new PositionedShape(Shapes.Line, new(X: 0, Y: 3));

        state1.Merge(shape);

        Assert.That(state1, Is.EqualTo(state2));
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

        var state = new RectangleGame2dState(stateSchema);

        var shape = new PositionedShape(Shapes.Line, new(0, RectangleGame2dState.TopIndex));

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMove(shape, PositionSpan.Right), Is.True);
            Assert.That(state.CanMove(shape, PositionSpan.Left), Is.False);
            Assert.That(state.CanMove(shape, PositionSpan.Down), Is.True);
        });
    }

    [Test]
    public void SingleLineIsCleared()
    {
        const string state1Schema = """
                                   --------
                                   --------
                                   --------
                                   ********
                                   """;

        const string state2Schema = """
                                   --------
                                   --------
                                   --------
                                   --------
                                   """;

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = new RectangleGame2dState(state2Schema);

        state1.ClearCompleteRows();

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void SingleLineIsClearedAndSomethingRemained()
    {
        const string state1Schema = """
                                   --------
                                   --*-----
                                   -**-----
                                   ********
                                   """;

        const string state2Schema = """
                                   --------
                                   --------
                                   --*-----
                                   -**-----
                                   """;

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = new RectangleGame2dState(state2Schema);

        state1.ClearCompleteRows();

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void TestRemoveLine()
    {
        const string state1Schema = """
                                    --------
                                    --*-----
                                    -**-----
                                    ***-----
                                    """;

        const string state2Schema = """
                                    --------
                                    --------
                                    --*-----
                                    -**-----
                                    """;

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = new RectangleGame2dState(state2Schema);

        state1.RemoveRow(0);

        Assert.That(state1, Is.EqualTo(state2));
    }
}
