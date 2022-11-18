using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Utility
{
    internal class UtilityFunctions
    {
        private const int FractionLength = 10;
        public string RoundFraction(string target)
        {
            if (target == null) return null;

            if (target.Contains('.') && target.Length - target.IndexOf('.') > FractionLength)
            {
                return target.Substring(0, target.IndexOf(".") + FractionLength + 1);
            }
            else
            {
                return target;
            }         
        }
    }
}
