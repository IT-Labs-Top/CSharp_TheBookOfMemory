using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TheBookOfMemory.Models.Enums;

namespace TheBookOfMemory.Converters;

public class EnumToVisibility : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (targetType != typeof(Visibility) || value == null || parameter == null)
            return Visibility.Collapsed;

        return value switch
        {
            ModeType mode when parameter is ModeType parameterModeType => mode == parameterModeType
                ? Visibility.Visible
                : Visibility.Collapsed,
            WriteType writeMode when parameter is WriteType parameterWriteMode => writeMode == parameterWriteMode
                ? Visibility.Visible
                : Visibility.Collapsed,
            _ => Visibility.Collapsed
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}