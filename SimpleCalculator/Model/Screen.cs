using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{    
    using uf = Utility.UtilityFunctions;
    internal class Screen
    {
        private string _result;
        private string _binaryOperator;
        private ValidityType _validity;
        public string Result 
        { 
            get { return uf.SetScientificNotation(uf.RoundDownFraction(_result)); } 
            set { _result = value; }           
        }
        public string BinaryOperator
        {
            get { return _binaryOperator; }
            set { _binaryOperator = value; }
        }
   
        public ValidityType Validity
        {
            get { return _validity; }
            set { _validity = value; }
        }
    public Screen() 
        { 
            _result = "0";
            _binaryOperator = null;
            _validity = ValidityType.VALID;
        }
        public Screen(string result, string binaryOperator, ValidityType validity)
        {
            _result = result;
            _binaryOperator = binaryOperator;
            _validity = validity;
        }
    }
}
