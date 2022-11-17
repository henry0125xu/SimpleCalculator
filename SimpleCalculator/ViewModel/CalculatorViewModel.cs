using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.ViewModel
{
    using Model;
    using Utility;
    using System.IO.Packaging;
    using System.Windows.Input;
    using System.Diagnostics;
    using System.Windows.Controls;

    internal class CalculatorViewModel : ViewModelBase
    {
        private Calculator _calculator;     
        private Screen _screenData;
        private ICommand _buttonCommand;

        public Screen ScreenData
        {
            get { return _screenData; }
            set 
            { 
                _screenData = value;
                OnPropertyChanged(nameof(ScreenData));
            }
        } 
        public ICommand ButtonCommand
        {
            get { return _buttonCommand; }
            set
            {
                _buttonCommand = value;
            }
        }
        public CalculatorViewModel()
        {
            _calculator = new Calculator();
            _screenData = new Screen();
   
            ButtonCommand = new RelayCommand(new Action<object>(ButtonHandler));
        }

        public void ButtonHandler(object buttonName)
        {
            string bn = buttonName.ToString();
            switch (bn)
            {
                case "0":
                case "1":                    
                case "2":                    
                case "3":                    
                case "4":                   
                case "5":                   
                case "6":                    
                case "7":                    
                case "8":              
                case "9":
                    _operandHandler(bn);
                    break;
                case "+":                   
                case "-":                    
                case "*":                   
                case "/":
                    _binaryOperatorHandler(bn);
                    break;
                case "1/x":
                case "sqrt":
                case "square":
                case "+/-":
                case "%":
                    _unaryOperatorHandler(bn);
                    break;
                case "C":
                case "CE":
                case "=":
                case ".":
                case "Back":
                    _functionalButtonHandler(bn);
                    break;
            }
        }

        private void _operandHandler(string op) 
        {
            CalculatorState newState;
            Screen newScreenData;

            if (_calculator.State.BinaryOperator == string.Empty)
            {
                string newFirstNumber;
                if (_calculator.State.FirstNumber == string.Empty || _calculator.State.FirstNumber == "0")
                {
                    newFirstNumber = op;
                }
                else
                {
                    newFirstNumber = _calculator.State.FirstNumber + op;
                }
                       
                newState = new CalculatorState(newFirstNumber, _calculator.State.BinaryOperator, _calculator.State.LastNumber);
                newScreenData = new Screen(newFirstNumber, _screenData.CurrBinaryOperator, false);
            }
            else 
            {
                string newLastNumber;
                if (_calculator.State.LastNumber == string.Empty || _calculator.State.LastNumber == "0")
                {
                    newLastNumber = op;
                }
                else
                {
                    newLastNumber = _calculator.State.LastNumber + op;
                }
                newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, newLastNumber);
                newScreenData = new Screen(newLastNumber, _screenData.CurrBinaryOperator, false);
            }

            Calculator newCalculator = new Calculator(newState);
            _calculator = newCalculator;
            ScreenData = newScreenData;
        }
        private void _unaryOperatorHandler(string op) 
        {
            CalculatorState newState;
            Screen newScreenData;

            if(_calculator.State.LastNumber == string.Empty && _calculator.State.BinaryOperator == string.Empty)
            {
                double firstNumber = Convert.ToDouble(_calculator.State.FirstNumber);
                switch (op)
                {
                    case "1/x":
                        firstNumber = 1 / firstNumber;
                        break;
                    case "sqrt":
                        firstNumber = Math.Sqrt(firstNumber);
                        break;
                    case "square":
                        firstNumber = Math.Pow(firstNumber, 2);
                        break;
                    case "+/-":
                        firstNumber *= -1;
                        break;
                    case "%":
                        firstNumber /= 100;
                        break;
                }
                string newFirstNumber = firstNumber.ToString();
                newState = new CalculatorState(newFirstNumber, _calculator.State.BinaryOperator, _calculator.State.LastNumber);
                newScreenData = new Screen(newFirstNumber, _screenData.CurrBinaryOperator, false);
            }
            else if (_calculator.State.LastNumber != string.Empty)
            {
                double lastNumber = Convert.ToDouble(_calculator.State.LastNumber);
                switch (op)
                {
                    case "1/x":
                        lastNumber = 1 / lastNumber;
                        break;
                    case "sqrt":
                        lastNumber = Math.Sqrt(lastNumber);
                        break;
                    case "square":
                        lastNumber = Math.Pow(lastNumber, 2);
                        break;
                    case "+/-":
                        if (_calculator.State.LastNumber != "0") lastNumber *= -1;
                        else lastNumber = 0;
                        break;
                    case "%":
                        lastNumber /= 100;
                        break;
                }
                string newLastNumber = lastNumber.ToString();
                newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, newLastNumber);
                newScreenData = new Screen(newLastNumber, _screenData.CurrBinaryOperator, false);
            }
            else
            {
                newState = _calculator.State;
                newScreenData = new Screen(_screenData.Result, _screenData.CurrBinaryOperator, true);
            }

            Calculator newCalculator = new Calculator(newState);
            _calculator = newCalculator;
            ScreenData = newScreenData;
        }
        private void _binaryOperatorHandler(string op) 
        {
            CalculatorState newState;
            Screen newScreenData;

            if(_calculator.State.LastNumber == string.Empty)
            {
                newState = new CalculatorState(_calculator.State.FirstNumber, op, _calculator.State.LastNumber);
                newScreenData = new Screen(_calculator.State.FirstNumber, op, _screenData.InValidOperation);
            }
            else
            {
                double res = new();
                switch (_calculator.State.BinaryOperator)
                {
                    case "+":
                        res = Convert.ToDouble(_calculator.State.FirstNumber) + Convert.ToDouble(_calculator.State.LastNumber);
                        break;
                    case "-":
                        res = Convert.ToDouble(_calculator.State.FirstNumber) - Convert.ToDouble(_calculator.State.LastNumber);
                        break;
                    case "*":
                        res = Convert.ToDouble(_calculator.State.FirstNumber) * Convert.ToDouble(_calculator.State.LastNumber);
                        break;
                    case "/":
                        res = Convert.ToDouble(_calculator.State.FirstNumber) / Convert.ToDouble(_calculator.State.LastNumber);            
                        break;
                }

                
                if (Double.IsInfinity(res)) // dividing by 0
                {
                    newState = _calculator.State;
                    newScreenData = new Screen(_screenData.Result, _screenData.CurrBinaryOperator, true);
                }
                else
                {
                    string newFirstNumber = res.ToString();
                    newState = new CalculatorState(newFirstNumber, op, string.Empty);
                    newScreenData = new Screen(newFirstNumber, op, false);
                }               
            }


            Calculator newCalculator = new Calculator(newState);
            _calculator = newCalculator;
            ScreenData = newScreenData;
        }
        private void _functionalButtonHandler(string op) 
        {
            CalculatorState newState = new CalculatorState();
            Screen newScreenData = new Screen();

            switch (op)
            {
                #region "C" Handler
                case "C":
                    break;
                #endregion
                #region "CE" Handler
                case "CE":                   
                    if (_calculator.State.BinaryOperator != string.Empty && _calculator.State.LastNumber == string.Empty)
                    {
                        newState = new CalculatorState(_calculator.State.FirstNumber, string.Empty, string.Empty);
                        newScreenData = new Screen(_screenData.Result, string.Empty, false);
                    }
                    else if(_calculator.State.LastNumber != string.Empty)
                    {
                        newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, string.Empty);
                        newScreenData = new Screen(string.Empty, _screenData.CurrBinaryOperator, false);                        
                    }
                    break;
                #endregion
                #region "=" Handler
                case "=":
                    _binaryOperatorHandler(string.Empty);
                    return;
                #endregion
                #region "." Handler
                case ".":
                    if (_calculator.State.BinaryOperator == string.Empty 
                        && _calculator.State.LastNumber == string.Empty
                        && _calculator.State.FirstNumber[_calculator.State.FirstNumber.Length - 1] != '.')
                    {
                        string newFirstNumber = _calculator.State.FirstNumber + ".";
                        newState = new CalculatorState(newFirstNumber, _calculator.State.BinaryOperator, _calculator.State.LastNumber);
                        newScreenData = new Screen(newFirstNumber, _screenData.CurrBinaryOperator, false);
                    }
                    else if(_calculator.State.LastNumber != string.Empty
                        && _calculator.State.LastNumber[_calculator.State.LastNumber.Length - 1] != '.')
                    {
                        string newLastNumber = _calculator.State.LastNumber + ".";
                        newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, newLastNumber);
                        newScreenData = new Screen(newLastNumber, _screenData.CurrBinaryOperator, false);
                    }
                    else
                    {
                        newState = _calculator.State;
                        newScreenData = new Screen(_screenData.Result, _screenData.CurrBinaryOperator, true);
                    }
                    break;
                #endregion
                #region "Back" Handler
                case "Back":
                    if(_calculator.State.BinaryOperator == string.Empty)
                    {
                        if(_calculator.State.FirstNumber.Length > 1)
                        {
                            string newFirstNumber = _calculator.State.FirstNumber.Remove(_calculator.State.FirstNumber.Length - 1);
                            newState = new CalculatorState(newFirstNumber, string.Empty, string.Empty);
                            newScreenData = new Screen(newFirstNumber, string.Empty, false); 
                        } 
                    }
                    else if(_calculator.State.BinaryOperator != string.Empty && _calculator.State.LastNumber == string.Empty)
                    {                                             
                        newState = new CalculatorState(_calculator.State.FirstNumber, string.Empty, string.Empty);
                        newScreenData = new Screen(_screenData.Result, string.Empty, false);                   
                    }
                    else
                    {
                        if (_calculator.State.FirstNumber.Length <= 1)
                        {
                            newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, string.Empty);
                            newScreenData = new Screen(string.Empty, _screenData.CurrBinaryOperator, false);
                        }
                        else
                        {
                            string newLastNumber = _calculator.State.LastNumber.Remove(_calculator.State.LastNumber.Length - 1);
                            newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, newLastNumber);
                            newScreenData = new Screen(newLastNumber, _screenData.CurrBinaryOperator, false);
                        }
                    }
                    break;
                #endregion
            }

            Calculator newCalculator = new Calculator(newState);
            _calculator = newCalculator;
            ScreenData = newScreenData;
        }
    }
}
