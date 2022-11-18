using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{
    using Utility;
    internal class Screen
    {
        private string _result;
        private string _binaryOperator;
        private ValidType _validity;
        public string Result 
        { 
            get { return new UtilityFunctions().RoundFraction(_result); } 
            set { _result = value; }           
        }
        public string BinaryOperator
        {
            get { return _binaryOperator; }
            set { _binaryOperator = value; }
        }
   
        public ValidType Validity
        {
            get { return _validity; }
            set { _validity = value; }
        }
    public Screen() 
        { 
            _result = "0";
            _binaryOperator = null;
            _validity = ValidType.VALID;
        }
        public Screen(string result, string binaryOperator, ValidType validity)
        {
            _result = result;
            _binaryOperator = binaryOperator;
            _validity = validity;
        }
    }
}
