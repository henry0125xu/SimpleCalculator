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
            switch ((ValidType)value)
            {               
                case ValidType.INFINITY:
                    return "Infinity Error!";
                case ValidType.OVERFLOW:
                    return "Overflow Error!";
                case ValidType.NAN:
                    return "NaN Error!"; 
                case ValidType.INVALID_OPERATION:
                    return "Invalid Operation!";
                case ValidType.EXCEPTION:
                    return "An Exception Occurs!";
                default:
                    return string.Empty;
            }
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
            switch ((string)value)
            {
                case "+":
                    return (char)0x002B;
                case "-":
                    return (char)0x2212;
                case "*":
                    return (char)0x00D7;
                case "/":
                    return (char)0x00F7;
                default:
                    return string.Empty;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

    }

}
