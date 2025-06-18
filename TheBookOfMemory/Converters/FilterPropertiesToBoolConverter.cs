using System.Globalization;
using System.Windows.Data;
using TheBookOfMemory.Models.Records;

namespace TheBookOfMemory.Converters;

public class FilterPropertiesToBoolConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 6) return false;
        var selectedMedal = values[0] as Medal;
        var selectedRank = values[1] as Rank;
        var ageBefore = values[2] is double before ? before : 1900;
        var ageAfter = values[3] is double after ? after : DateTime.Now.Year;
        var minimal = values[4] is double minimalValue ? minimalValue : 1900;
        var maximal = values[5] is double maximalValue ? maximalValue : DateTime.Now.Year;

        return selectedMedal != null && selectedMedal?.Id != -1 ||
               selectedRank != null && selectedRank?.Id != -1 ||
               ageAfter != maximal ||
               ageBefore != minimal;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}