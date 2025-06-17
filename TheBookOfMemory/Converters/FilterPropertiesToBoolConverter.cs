using System.Globalization;
using System.Windows.Data;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Converters;

public class FilterPropertiesToBoolConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 4) return false;
        var selectedMedal = values[0] as Medal;
        var selectedRank = values[1] as Rank;
        var ageBefore = values[2] is int before ? before : 1900;
        var ageAfter = values[3] is int after ? after : DateTime.Now.Year;

        return selectedMedal != null && selectedMedal?.Id != -1 ||
               selectedRank != null && selectedRank?.Id != -1 ||
               ageAfter != DateTime.Now.Year ||
               ageBefore != 1900;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}