using System.Globalization;
using System.Windows.Data;

namespace TheBookOfMemory.Converters;

public class DoubleToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double numberValue) return 0.0;
        return numberValue.ToString();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}