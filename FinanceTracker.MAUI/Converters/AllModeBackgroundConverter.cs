using FinanceTracker.Core.ViewModels;
using Microsoft.Maui.Controls;
using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class AllModeBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value?.ToString() == "All"
            ? Colors.White
            : Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
