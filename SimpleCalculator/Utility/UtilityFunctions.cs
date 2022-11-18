using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Utility
{
    internal class UtilityFunctions
    {
        public string RoundFraction(string target)
        {
            if (target.Contains('.') && target.Length - target.IndexOf('.') > 10)
            {
                return target.Substring(0, target.IndexOf(".") + 10);
            }
            else
            {
                return target;
            }         
        }
    }
}
