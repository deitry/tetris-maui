using Tetris.CommonLib;

namespace TetrisTests;

public class RectangleGameStateTests
{
    [Test]
    public void Test001_LineCanBeSpawnedInEmptyState()
    {
        var stateSchema = """
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
        var stateSchema = """
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
        var state1Schema = """
                           --------
                           --------
                           --------
                           --------
                           """;

        var state2Schema = """
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
}
