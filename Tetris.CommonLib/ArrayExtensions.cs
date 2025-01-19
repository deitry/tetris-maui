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
}
