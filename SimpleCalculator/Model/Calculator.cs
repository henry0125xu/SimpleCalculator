using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.Model
{
    struct CalculatorState
    {
        public string FirstNumber { get; set; }
        public string LastNumber { get; set; }
        public string BinaryOperator { get; set; }
        public CalculatorState()
        {
            FirstNumber = "0";
            BinaryOperator = string.Empty;
            LastNumber = string.Empty;
        }
        public CalculatorState(string firstNumber, string binaryOperator, string lastNumber)
        {
            FirstNumber = firstNumber;
            BinaryOperator = binaryOperator;
            LastNumber = lastNumber;     
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
            State = new CalculatorState();         
        }
        public Calculator(CalculatorState state)
        {
            State = state;
        }
    }
}
