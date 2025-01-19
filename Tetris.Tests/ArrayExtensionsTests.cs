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
    public void TestWidth_NotEmptyArray1()
    {
        var array = new bool[4, 5];

        Assert.That(array.Width(), Is.EqualTo(0));
    }
}
