using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SimpleCalculator.Utility
{
    using Model;
    internal class ValidityConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (ValidType)value switch
            {
                ValidType.INFINITY => "Infinity Error!",
                ValidType.OVERFLOW => "Overflow Error!",
                ValidType.NAN => "NaN Error!",
                ValidType.INVALID_OPERATION => "Invalid Operation!",
                ValidType.EXCEPTION => "Some Exception Occurs!",
                _ => string.Empty,
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

    }

    internal class OperatorConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value switch
            {
                "+" => (char)0x002B,
                "-" => (char)0x2212,
                "*" => (char)0x00D7,
                "/" => (char)0x00F7,
                _ => string.Empty,
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

    }

}
