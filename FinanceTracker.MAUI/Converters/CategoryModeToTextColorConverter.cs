using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class CategoryModeToTextColorConverter : IValueConverter
{
    public CategoryViewMode TargetMode { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CategoryViewMode current)
            return current == TargetMode ? Colors.Black : Colors.Gray;

        return Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
