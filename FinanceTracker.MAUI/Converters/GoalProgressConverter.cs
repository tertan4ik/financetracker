using System.Globalization;

namespace FinanceTracker.MAUI.Converters;

public class GoalProgressConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not FinanceTracker.Core.Models.SavingGoal goal)
            return 0d;

        if (goal.TargetAmount <= 0)
            return 0d;

        var progress = (double)(goal.CurrentAmount / goal.TargetAmount);

        // ограничиваем 0..1
        return Math.Min(1.0, Math.Max(0.0, progress));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
