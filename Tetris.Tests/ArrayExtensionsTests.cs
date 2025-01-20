using Tetris.CommonLib;

namespace TetrisTests;

public class ArrayExtensionsTests
{
    [Test]
    public void TestStringConversions()
    {
        const string s = """
                         ----
                         ****
                         """;

        var a = s.To2dBoolArray();

        Assert.That(a.AsString(emptyPlaceholder: '-'), Is.EqualTo(s));
    }

    [Test]
    public void TestWidth_EmptyArray()
    {
        var array = new bool[4, 5];

        Assert.That(array.Width(), Is.EqualTo(0));
    }

    [Test]
    public void TestWidth_NotEmptyArray()
    {
        var array = new bool[4, 5];
        array[1, 2] = true;

        Assert.Multiple(() =>
        {
            Assert.That(array.Width(), Is.EqualTo(1));
            Assert.That(array.Height(), Is.EqualTo(1));
        });
    }

    [Test]
    public void TestDimensions_Line()
    {
        var shape = Shapes.Line;

        Assert.Multiple(() =>
        {
            Assert.That(shape.Width, Is.EqualTo(4));
            Assert.That(shape.Height, Is.EqualTo(1));
        });
    }

    [Test]
    public void TestDimensions_RotatedLine()
    {
        var shape = Shapes.Line.RotatedClockwise;

        Assert.Multiple(() =>
        {
            Assert.That(shape.Width, Is.EqualTo(1));
            Assert.That(shape.Height, Is.EqualTo(4));
        });
    }

    [Test]
    public void TestDimensions_Square()
    {
        var shape = Shapes.Square;

        Assert.Multiple(() =>
        {
            Assert.That(shape.Width, Is.EqualTo(2));
            Assert.That(shape.Height, Is.EqualTo(2));
        });
    }

    [Test]
    public void TestDimensions_RotatedSquare()
    {
        var shape = Shapes.Square;

        Assert.Multiple(() =>
        {
            Assert.That(shape.Width, Is.EqualTo(2));
            Assert.That(shape.Height, Is.EqualTo(2));
        });
    }
}
