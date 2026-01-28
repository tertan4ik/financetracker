using FinanceTracker.core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.MAUI.Converters
{
    public class IncomeBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Colors.Transparent;

            var modeName = value.ToString();

            return modeName == "Income"
                ? Colors.White
                : Colors.Transparent;
        }
        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    System.Diagnostics.Debug.WriteLine(
        //        $"[IncomeBackgroundConverter] value = {value}, type = {value?.GetType()}");

        //    return Colors.Red; // <-- СПЕЦИАЛЬНО КРАСНЫЙ
        //}
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }


}
