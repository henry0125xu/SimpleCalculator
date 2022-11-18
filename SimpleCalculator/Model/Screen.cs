using SimpleCalculator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{ 
    internal class Screen
    {
        private string _result;
        private string _currBinaryOperator;
        private bool _inValidOperation;
        public string Result 
        { 
            get { return new UtilityFunctions().RoundFraction(_result); } 
            set { _result = value; }           
        }
        public string CurrBinaryOperator
        {
            get { return _currBinaryOperator; }
            set { _currBinaryOperator = value; }
        }
   
        public bool InValidOperation
        {
            get { return _inValidOperation; }
            set { _inValidOperation = value; }
        }
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
