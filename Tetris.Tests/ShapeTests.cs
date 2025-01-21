using Tetris.CommonLib;

namespace TetrisTests;

public class ShapeTests
{
    [Test]
    public void TestLineAsString()
    {
        var line = Shapes.Line;

        Assert.That(line.ToString()!.TrimEnd('\r', '\n', ' '), Is.EqualTo("****"));
    }

    [Test]
    public void CheckTwoLinesEqual()
    {
        var line1 = Shapes.Line;
        var line2 = Shapes.Line;

        Assert.That(line1, Is.EqualTo(line2));
    }

    [Test]
    public void CheckTwoShapesNotEqual()
    {
        var line1 = Shapes.Line;
        var line2 = Shapes.Square;

        Assert.That(line1, Is.Not.EqualTo(line2));
    }

    [Test]
    public void RotateLine()
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
    public void RotateLine360()
    {
        var line = Shapes.Line;
        var rotated90 = line.RotatedClockwise;
        var rotated180 = rotated90.RotatedClockwise;
        var rotated270 = rotated180.RotatedClockwise;
        var rotated360 = rotated270.RotatedClockwise;

        Assert.That(rotated360, Is.EqualTo(line));
    }

    [Test]
    public void RotateTShape()
    {
        var t = Shapes.T;
        var rotated90 = t.RotatedClockwise;
        var rotated180 = rotated90.RotatedClockwise;
        var rotated270 = rotated180.RotatedClockwise;
        var rotated360 = rotated270.RotatedClockwise;

        Assert.Multiple(() =>
        {
            Assert.That(rotated90, Is.EqualTo(new Shape("""
                                                    ---*
                                                    --**
                                                    ---*
                                                    ----
                                                    """)));

            Assert.That(rotated180, Is.EqualTo(new Shape("""
                                                    ----
                                                    ----
                                                    --*-
                                                    -***
                                                    """)));

            Assert.That(rotated270, Is.EqualTo(new Shape("""
                                                    ----
                                                    *---
                                                    **--
                                                    *---
                                                    """)));

            Assert.That(rotated360, Is.EqualTo(t));
        });
    }
}
