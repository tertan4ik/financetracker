using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class BalanceColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal balance)
        {
            if (balance > 0)
                return Color.FromArgb("#2ECC71"); // зеленый
            else if (balance < 0)
                return Color.FromArgb("#E74C3C"); // красный
            else
                return Color.FromArgb("#333333"); // темно-серый
        }
        return Color.FromArgb("#333333");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}