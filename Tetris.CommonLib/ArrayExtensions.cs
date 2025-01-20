using System.Text;

namespace Tetris.CommonLib;

public static class ArrayExtensions
{
    public static T[,] DeepClone<T>(this T[,] array)
    {
        var clone = new T[array.GetLength(0), array.GetLength(1)];
        for (var i = 0; i < array.GetLength(0); i++)
        {
            for (var j = 0; j < array.GetLength(1); j++)
            {
                clone[i, j] = array[i, j];
            }
        }

        return clone;
    }

    /// <summary>
    /// Returns max width of occupied cells of array
    /// </summary>
    public static int Width(this bool[,] array)
    {
        var minCol = int.MaxValue;
        var maxCol = int.MinValue;

        for (var row = 0; row < array.GetLength(1); row++)
        {
            for (var col = 0; col < array.GetLength(0); col++)
            {
                if (array[col, row])
                {
                    minCol = Math.Min(minCol, col);
                    maxCol = Math.Max(maxCol, col);
                }
            }
        }

        if (minCol == int.MaxValue && maxCol == int.MinValue)
            return 0;

        return maxCol - minCol + 1;
    }

    /// <summary>
    /// Returns max width of occupied cells of array
    /// </summary>
    public static int Height(this bool[,] array)
    {
        var minRow = int.MaxValue;
        var maxRow = int.MinValue;

        for (var row = 0; row < array.GetLength(1); row++)
        {
            for (var col = 0; col < array.GetLength(0); col++)
            {
                if (array[col, row])
                {
                    minRow = Math.Min(minRow, row);
                    maxRow = Math.Max(maxRow, row);
                }
            }
        }

        if (minRow == int.MaxValue && maxRow == int.MinValue)
            return 0;

        return maxRow - minRow + 1;
    }

    public static bool[,] To2dBoolArray(this string stringRepresentation)
    {
        const string LineEnding = "\n";
        var lines = stringRepresentation
            .ReplaceLineEndings(LineEnding)
            .Split(LineEnding);

        var width = lines.Max(l => l.Length);
        var height = lines.Length;

        var array = new bool[width, height];

        for (var col = 0; col < width; col++)
        {
            for (var row = 0; row < height && col < lines[row].Length; row++)
            {
                array[col, row] = lines[row][col] == '*';
            }
        }

        return array;
    }

    public static string AsString(this bool[,] array, char emptyPlaceholder = ' ', char occupiedPlaceholder = '*')
    {
        var sb = new StringBuilder();
        for (var row = 0; row < array.GetLength(1); row++)
        {
            for (var col = 0; col < array.GetLength(0); col++)
            {
                sb.Append(array[col, row] ? occupiedPlaceholder : emptyPlaceholder);
            }

            sb.AppendLine();
        }

        return sb.ToString().TrimEnd(['\r', '\n']);
    }
}
