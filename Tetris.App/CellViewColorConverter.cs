using System.Globalization;

namespace TetrisApp;

public class CellViewColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is CellState state)
        {
            return state switch
            {
                CellState.Moving => Colors.Red,
                CellState.Static => Colors.Gold,
                _ => Colors.White,
            };
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
