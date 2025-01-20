using Tetris.CommonLib;

namespace TetrisTests;

public class RectangleGame2dStateTests
{
    [Test]
    public void LineCanBeSpawnedInEmptyState()
    {
        var state = StateManager.Empty9x4;

        Assert.That(state.CanSpawn(Shapes.Line), Is.True);
    }

    [Test]
    public void LineCanNotBeSpawnedInFullState()
    {
        var state = StateManager.Full9x4;

        Assert.That(state.CanSpawn(Shapes.Line), Is.False);
    }

    [Test]
    public void LineIsMergedIntoState()
    {
        var state1 = StateManager.Empty9x4;

        var state2 = new RectangleGame2dState("""
                                              --------
                                              --------
                                              --------
                                              ****----
                                              """);

        var shape = new PositionedShape(Shapes.Line, new(X: 0, Y: 3));

        state1.Merge(shape);

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void CanMoveLine_LeftTop()
    {
        var state = StateManager.Empty9x4;

        var shape = new PositionedShape(Shapes.Line, new(state.LeftIndex, state.TopIndex));

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMove(shape, PositionSpan.Right), Is.True);
            Assert.That(state.CanMove(shape, PositionSpan.Left), Is.False);
            Assert.That(state.CanMove(shape, PositionSpan.Down), Is.True);
        });
    }

    [Test]
    public void CanMoveLine_LeftBottom()
    {
        var state = StateManager.Empty9x4;

        var shape = new PositionedShape(Shapes.Line, new(state.LeftIndex, state.BottomIndex));

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMove(shape, PositionSpan.Right), Is.True);
            Assert.That(state.CanMove(shape, PositionSpan.Left), Is.False);
            Assert.That(state.CanMove(shape, PositionSpan.Down), Is.False);
        });
    }

    [Test]
    public void CanMoveLine_RightBottom()
    {
        var state = StateManager.Empty9x4;

        var line = Shapes.Line;
        var shape = new PositionedShape(line, new(state.Width - line.Width, state.BottomIndex));

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMove(shape, PositionSpan.Right), Is.False);
            Assert.That(state.CanMove(shape, PositionSpan.Left), Is.True);
            Assert.That(state.CanMove(shape, PositionSpan.Down), Is.False);
        });
    }

    [Test]
    public void CanRotateLine()
    {
        var state = StateManager.Empty9x4;
        var line = Shapes.Line;
        var shape = new PositionedShape(line, new(state.LeftIndex, state.TopIndex));

        Assert.That(state.CanMerge(shape.Shape.RotatedClockwise, shape.Position), Is.True);
    }

    [Test]
    public void SingleRowIsCleared()
    {
        const string state1Schema = """
                                   --------
                                   --------
                                   --------
                                   ********
                                   """;

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = StateManager.Empty9x4;

        state1.ClearCompleteRows();

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void SingleRowNonBottomIsCleared()
    {
        const string state1Schema = """
                                   --------
                                   --------
                                   ********
                                   --------
                                   """;

        var state1 = new RectangleGame2dState(state1Schema);
        var state2 = StateManager.Empty9x4;

        state1.ClearCompleteRows();

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void SingleRowIsClearedAndSomethingRemained()
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
    public void RowRemovesProperly()
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

        state1.RemoveRow(state1.BottomIndex);

        Assert.That(state1, Is.EqualTo(state2));
    }

    [Test]
    public void CanPlaceLShape()
    {
        var state = StateManager.Empty9x4;
        var l = Shapes.L;

        Assert.Multiple(() =>
        {
            Assert.That(state.CanMerge(l, new(0, 0)), Is.True);
            Assert.That(state.CanMerge(l, new(0, 2)), Is.True);
            Assert.That(state.CanMerge(l, new(1, 2)), Is.True);
            Assert.That(state.CanMerge(l, new(3, 2)), Is.True);

            Assert.That(state.CanMerge(l, new(6, 2)), Is.False);
            Assert.That(state.CanMerge(l, new(0, 3)), Is.False);
        });
    }
}
