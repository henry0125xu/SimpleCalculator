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
        public bool HaveDot { get; set; }
        public CalculatorState()
        {
            FirstNumber = string.Empty;
            BinaryOperator = string.Empty;
            LastNumber = string.Empty;
            HaveDot = false;
        }
        public CalculatorState(string firstNumber, string binaryOperator, string lastNumber, bool haveDot)
        {
            FirstNumber = firstNumber;
            BinaryOperator = binaryOperator;
            LastNumber = lastNumber;     
            HaveDot = haveDot;
        }
    };
    internal class Calculator
    {
        public CalculatorState State { get; set; }
        public Screen ScreenData { get; set; }
        public Calculator() 
        {
            State = new CalculatorState();
            ScreenData = new Screen();
        }
        public Calculator(CalculatorState state, Screen screenData)
        {
            State = state;
            ScreenData = screenData;
        }
    }
}
