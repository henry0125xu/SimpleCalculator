using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Utility
{
    internal class UtilityFunctions
    {
        /* Reserve the first FractionLength fractions */
        private const int FractionLength = 13;
        public static string RoundDownFraction(string target)
        {        
            if (target == null) return null;

            /* Is scientific notation */
            if (target.Contains('E')) return target;


            /* Is floating point */
            if (target.Contains('.') && target.Length - target.IndexOf('.') > FractionLength)
            {
                return target[..(target.IndexOf(".") + FractionLength + 1)];
            }
            else
            {
                return target;
            }         
        }

        public static string SetScientificNotation(string target)
        {
            if (target == null) return null;

            /* The target number is too small */
            if (target.Contains("E-") && target.Length - target.IndexOf('E') > 5) return "0";

            /* The target number is big enough && already in scientific notation */
            if (target.Contains('E')) return target;

            /* Is integer */
            if (!target.Contains('.'))
            {
                if (target.Length < 25) return target;
                else
                {
                    return Convert.ToDouble(target).ToString();
                }
            }
            /* Is floating point */
            else
            {
                if(target.IndexOf('.') < 20) return target;
                else
                {
                    return Convert.ToDouble(target).ToString();
                }
            }                  
        }
    }
}
