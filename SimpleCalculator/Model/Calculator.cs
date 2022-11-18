using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{
    using Utility;
    struct CalculatorState
    {
        private string _firstNumber;
        private string _binaryOperator;
        private string _lastNumber;
        
        public string FirstNumber
        {
            get { return new UtilityFunctions().RoundFraction(_firstNumber); }           
            set { _firstNumber = value; }
        }
        public string BinaryOperator
        {
            get { return _binaryOperator; }
            set { _binaryOperator = value; }
        }
        public string LastNumber
        {
            get { return new UtilityFunctions().RoundFraction(_lastNumber); }
            set { _lastNumber = value; }
        }
        
        public CalculatorState()
        {
            _firstNumber = "0";
            _binaryOperator = null;
            _lastNumber = null;
        }
        public CalculatorState(string firstNumber, string binaryOperator, string lastNumber)
        {
            _firstNumber = firstNumber;
            _binaryOperator = binaryOperator;
            _lastNumber = lastNumber;     
        }
    };
    internal class Calculator
    {
        private CalculatorState _state;
        public CalculatorState State
        {
            get { return _state; }
            set { _state = value; }
        }
        public Calculator() 
        {
            _state = new CalculatorState();         
        }
        public Calculator(CalculatorState state)
        {
            _state = state;
        }
    }
}
