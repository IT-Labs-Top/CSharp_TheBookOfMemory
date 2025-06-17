using System.Globalization;
using System.Windows.Data;
using TheBookOfMemory.Models.Entities;

namespace TheBookOfMemory.Converters;

public class ModelToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Filter filter) return false;

        return filter.SelectedMedal != null ||
               filter.SelectedRank != null ||
               filter.AgeAfter != 1900 ||
               filter.AgeBefore != DateTime.Now.Year;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}