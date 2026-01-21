using FinanceTracker.core.Enums;
using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class CategoryTypeColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is CategoryType type
            ? type == CategoryType.Доход
                ? Colors.Green
                : Colors.Red
            : Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
