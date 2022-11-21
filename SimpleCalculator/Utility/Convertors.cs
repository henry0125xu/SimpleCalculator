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
            return (ValidityType)value switch
            {               
                ValidityType.DIVIDE_BY_ZERO => "Cannot Divide by 0!",
                ValidityType.OVERFLOW => "Overflow!",
                ValidityType.NAN => "NaN Error!",
                ValidityType.INVALID_OPERATION => "Invalid Operation!",
                ValidityType.EXCEPTION => "Some Exception Occurs!",
                _ => string.Empty,
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }

    /* Change display font style */
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
            return null;
        }
    }

    /* Font size depends on Result.Length */
    internal class FontSizeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value < 15) return 50;
            else if ((int)value < 20) return 40;
            else if ((int)value < 25) return 30;
            else if ((int)value < 30) return 25;
            else return 20;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
