using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class BalanceSignConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal balance)
        {
            // Возвращаем строку с числом и знаком
            if (balance > 0)
                return $"+{balance:N0} ₽";
            else if (balance < 0)
                return $"-{Math.Abs(balance):N0} ₽";
            else
                return $"0 ₽";
        }
        return "0 ₽";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}