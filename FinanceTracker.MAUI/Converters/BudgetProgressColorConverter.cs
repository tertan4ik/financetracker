using Microsoft.Maui.Controls;
using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class BudgetProgressColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool isExceeded)
            return isExceeded ? Colors.Red : Color.FromArgb("#2ECC71");

        return Color.FromArgb("#2ECC71");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
