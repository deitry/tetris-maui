using Tetris.CommonLib;

namespace TetrisTests;

public class ShapeTests
{
    [Test]
    public void Test001_CheckTwoLinesEqual()
    {
        var line1 = Shapes.Line;
        var line2 = Shapes.Line;

        Assert.That(line1, Is.EqualTo(line2));
    }

    [Test]
    public void Test002_CheckTwoShapesNotEqual()
    {
        var line1 = Shapes.Line;
        var line2 = Shapes.Square;

        Assert.That(line1, Is.Not.EqualTo(line2));
    }
}
