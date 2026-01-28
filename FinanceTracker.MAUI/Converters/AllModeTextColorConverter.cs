using Microsoft.Maui.Controls;
using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class AllModeTextColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString() == "All"
            ? Colors.Black
            : Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
