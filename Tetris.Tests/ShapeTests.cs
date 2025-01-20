using Tetris.CommonLib;

namespace TetrisTests;

public class ShapeTests
{
    [Test]
    public void TestLineAsString()
    {
        var line = Shapes.Line;

        Assert.That(line.ToString()!.TrimEnd(['\r', '\n', ' ']), Is.EqualTo("****"));
    }

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

    [Test]
    public void Test003_RotateLine()
    {
        var line = Shapes.Line;
        var rotatedLine = new Shape("""
                                    *
                                    *
                                    *
                                    *
                                    """);

        Assert.That(line.RotatedClockwise, Is.EqualTo(rotatedLine));
    }

    [Test]
    public void Test004_RotateLine360()
    {
        var line = Shapes.Line;
        var rotated90 = line.RotatedClockwise;
        var rotated180 = rotated90.RotatedClockwise;
        var rotated270 = rotated180.RotatedClockwise;
        var rotated360 = rotated270.RotatedClockwise;

        Assert.That(rotated360, Is.EqualTo(line));
    }
}
