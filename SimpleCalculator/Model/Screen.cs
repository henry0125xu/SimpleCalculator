using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{
    internal class Screen
    {
        public string Result { get; set; }
        public string CurrBinaryOperator { get; set; }
        public bool InValidOperation { get; set; }
        public Screen() 
        { 
            Result = "0";
            CurrBinaryOperator = string.Empty;
            InValidOperation = false;
        }
        public Screen(string result, string currBinaryOperator, bool inValidOperation)
        {
            Result = result;
            CurrBinaryOperator = currBinaryOperator;
            InValidOperation = inValidOperation;
        }
    }
}
