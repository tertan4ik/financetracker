using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class CategoryModeToColorConverter : IValueConverter
{
    public CategoryViewMode TargetMode { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is CategoryViewMode current)
            return current == TargetMode ? Colors.White : Colors.Transparent;

        return Colors.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
