using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPU.Utils.GUIUtils
{
    public class IsNullToBooleanConverter : IValueConverter
    {
        public bool IsNotNullValue { get; set; } = true;
        public bool IsNullValue { get; set; } = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? IsNullValue : IsNotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
