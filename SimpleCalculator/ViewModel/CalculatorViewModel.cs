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

                // Console.WriteLine(_calculator.State.FirstNumber);

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

        public void ButtonHandler(object operation)
        {
            try
            {
                string op = operation.ToString();
                switch (op)
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
                        _operandHandler(op);
                        break;
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        _binaryOperatorHandler(op);
                        break;
                    case "1/x":
                    case "sqrt":
                    case "square":
                    case "+/-":
                    case "%":
                        _unaryOperatorHandler(op);
                        break;
                    case "C":
                    case "CE":
                    case "=":
                    case ".":
                    case "Back":
                        _functionalButtonHandler(op);
                        break;
                }
            }
            catch (Exception ex) 
            {
                // Console.WriteLine(ex.ToString());

                _functionalButtonHandler("C");
                _setInvalidOperation(ValidType.EXCEPTION);
            }                     
        }

        private void _operandHandler(string op) 
        {
            /* Currently handling the first number */
            if (_calculator.State.BinaryOperator == null && _calculator.State.LastNumber == null)  
            {
                string newFirstNumber;
                /* The first number = 0 */
                if (_calculator.State.FirstNumber == null || _calculator.State.FirstNumber == "0")
                {
                    newFirstNumber = op;
                }
                /* The first number != 0 */
                else
                {
                    newFirstNumber = _calculator.State.FirstNumber + op;
                }

                /* Check the validity */
                if (_validType(Convert.ToDouble(newFirstNumber)) != ValidType.VALID)
                {
                    _setInvalidOperation(_validType(Convert.ToDouble(newFirstNumber)));
                }
                else
                {
                    _updateCalculator(true, false, false, newFirstNumber, null, null);
                    _updateScreenData(true, false, true, newFirstNumber, null, ValidType.VALID);                    
                }            
            }
            /* Currently handling the last number */
            else
            {
                string newLastNumber;
                /* The last number = 0 */
                if (_calculator.State.LastNumber == null || _calculator.State.LastNumber == "0")
                {
                    newLastNumber = op;
                }
                /* The last number != 0 */
                else
                {
                    newLastNumber = _calculator.State.LastNumber + op;
                }


                /* Check the validity */
                if (_validType(Convert.ToDouble(newLastNumber)) != ValidType.VALID)
                {
                    _setInvalidOperation(_validType(Convert.ToDouble(newLastNumber)));
                }
                else
                {
                    _updateCalculator(false, false, true, null, null, newLastNumber);
                    _updateScreenData(true, false, true, newLastNumber, null, ValidType.VALID);
                }
                    
            }
        }
        private void _unaryOperatorHandler(string op) 
        {
            /* Currently handling the first number */
            if (_calculator.State.BinaryOperator == null && _calculator.State.LastNumber == null)
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
                        if (_calculator.State.FirstNumber != null && 
                            _calculator.State.FirstNumber != "0") firstNumber *= -1;
                        else firstNumber = 0;
                        break;
                    case "%":
                        firstNumber /= 100;
                        break;
                }


                /* Check the validity */
                if (_validType(firstNumber) != ValidType.VALID) 
                {
                    _setInvalidOperation(_validType(firstNumber));
                }
                else
                {
                    string newFirstNumber = firstNumber.ToString();
                    _updateCalculator(true, false, false, newFirstNumber, null, null);
                    _updateScreenData(true, false, true, newFirstNumber, null, ValidType.VALID);
                }                    
            }
            /* Currently handling the last number */
            else if (_calculator.State.LastNumber != null)
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
                        if (_calculator.State.LastNumber != null && 
                            _calculator.State.LastNumber != "0") lastNumber *= -1;
                        else lastNumber = 0;
                        break;
                    case "%":
                        lastNumber /= 100;
                        break;
                }


                /* Check the validity */
                if (_validType(lastNumber) != ValidType.VALID) 
                {
                    _setInvalidOperation(_validType(lastNumber));
                }
                else
                {
                    string newLastNumber = lastNumber.ToString();
                    _updateCalculator(false, false, true, null, null, newLastNumber);
                    _updateScreenData(true, false, true, newLastNumber, null, ValidType.VALID);
                }                   
            }
            /* Currently existing a binary operator  */
            else
            {
                _setInvalidOperation(ValidType.INVALID_OPERATION);
            }
        }
        private void _binaryOperatorHandler(string op) 
        {
            /* The last number does not currently exist */
            if (_calculator.State.LastNumber == null)
            {
                _updateCalculator(false, true, false, null, op, null);
                _updateScreenData(false, true, false, null, op, ValidType.VALID);
            }
            /* The last number currently exist, calculate the current expression */
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


                /* Check the validity */
                if (_validType(res) != ValidType.VALID) // something wrong
                {
                    _setInvalidOperation(_validType(res));
                }
                else
                {
                    string newFirstNumber = res.ToString();
                    _updateCalculator(true, true, true, newFirstNumber, op, null);
                    _updateScreenData(true, true, true, newFirstNumber, op, ValidType.VALID);
                }               
            }
        }
        private void _functionalButtonHandler(string op) 
        {
            switch (op)
            {
                #region "C" Handler
                /* Clear all */
                case "C":
                    _updateCalculator(true, true, true, "0", null, null);
                    _updateScreenData(true, true, true, "0", null, ValidType.VALID);
                    break;
                #endregion
                #region "CE" Handler
                /* Clear error */
                case "CE":
                    /* Currently handling the first number */
                    if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        _updateCalculator(false, true, true, null, null, null);
                        _updateScreenData(false, true, true, null, null, ValidType.VALID);
                    }
                    /* Currently handling the last number */
                    else if (_calculator.State.LastNumber != null)
                    {
                        _updateCalculator(false, false, true, null, null, null);
                        _updateScreenData(true, false, true, null, null, ValidType.VALID);                       
                    }
                    break;
                #endregion
                #region "=" Handler
                    /* Calulate the current expression */
                case "=":
                    /* The last number does not currently exist */
                    if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        _setInvalidOperation(ValidType.INVALID_OPERATION);
                    }
                    else
                    {
                        _binaryOperatorHandler(null);
                        return;
                    }
                    break;
                #endregion
                #region "." Handler
                /* Decimal point */
                case ".":
                    /* Currently handling the first number and decimal point does not exist  */
                    if (_calculator.State.BinaryOperator == null
                        && _calculator.State.LastNumber == null
                        && !_calculator.State.FirstNumber.Contains('.'))
                    {
                        string newFirstNumber = _calculator.State.FirstNumber + ".";
                        _updateCalculator(true, false, false, newFirstNumber, null, null);
                        _updateScreenData(true, false, true, newFirstNumber, null, ValidType.VALID);
                    }
                    /* Currently handling the last number and decimal point does not exist  */
                    else if (_calculator.State.LastNumber != null
                        && !_calculator.State.LastNumber.Contains('.'))
                    {
                        string newLastNumber = _calculator.State.LastNumber + ".";
                        _updateCalculator(false, false, true, null, null, newLastNumber);
                        _updateScreenData(true, false, true, newLastNumber, null, ValidType.VALID);
                    }
                    /* Other cases are all invalid */
                    else
                    {
                        _setInvalidOperation(ValidType.INVALID_OPERATION);
                    }
                    break;
                #endregion
                #region "Back" Handler
                    /* Revise a digit or the current operator */
                case "Back":
                    /* Currently handling the first number */
                    if (_calculator.State.BinaryOperator == null && _calculator.State.LastNumber == null)
                    {
                        if(_calculator.State.FirstNumber.Length <= 1)
                        {
                            _updateCalculator(true, false, false, "0", null, null);
                            _updateScreenData(true, false, true, "0", null, ValidType.VALID);
                        }
                        else
                        {
                            string newFirstNumber = _calculator.State.FirstNumber.Remove(_calculator.State.FirstNumber.Length - 1);
                            if(newFirstNumber == "-")
                            {
                                _updateCalculator(true, false, false, "0", null, null);
                                _updateScreenData(true, false, true, "0", null, ValidType.VALID);
                            }
                            else
                            {
                                _updateCalculator(true, false, false, newFirstNumber, null, null);
                                _updateScreenData(true, false, true, newFirstNumber, null, ValidType.VALID);
                            }                           
                        } 
                    }
                    /* Currently handling the binary operator */
                    else if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        _updateCalculator(false, true, false, null, null, null);
                        _updateScreenData(false, true, true, null, null, ValidType.VALID);                  
                    }
                    /* Currently handling the last number */
                    else
                    {
                        if (_calculator.State.LastNumber.Length <= 1)
                        {
                            _updateCalculator(false, false, true, null, null, "0");
                            _updateScreenData(true, false, true, "0", null, ValidType.VALID);
                        }
                        else
                        {
                            string newLastNumber = _calculator.State.LastNumber.Remove(_calculator.State.LastNumber.Length - 1);
                            if(newLastNumber == "-")
                            {
                                _updateCalculator(false, false, true, null, null, "0");
                                _updateScreenData(true, false, true, "0", null, ValidType.VALID);
                            }
                            else
                            {
                                _updateCalculator(false, false, true, null, null, newLastNumber);
                                _updateScreenData(true, false, true, newLastNumber, null, ValidType.VALID);
                            }                           
                        }
                    }
                    break;
                #endregion
            }
        }
        
        /* The following are some helper functions */
        private void _updateCalculator(bool ufn, bool ubo, bool uln, string fn, string bn, string ln) 
        {           
            string newFirstNumber;
            string newBinaryOperator;
            string newLastNumber;

            if (ufn) newFirstNumber = fn;
            else newFirstNumber = _calculator.State.FirstNumber;

            if (ubo) newBinaryOperator = bn;
            else newBinaryOperator = _calculator.State.BinaryOperator;

            if (uln) newLastNumber = ln;
            else newLastNumber= _calculator.State.LastNumber;
          
            CalculatorState newState = new CalculatorState(newFirstNumber, newBinaryOperator, newLastNumber);
            Calculator newCalculator = new Calculator(newState);
            _calculator = newCalculator;           
        }
        private void _updateScreenData(bool urs, bool ucbo, bool uvd, string rs, string cbo, ValidType vd) 
        {
            string newResult;
            string newCurrBinaryOperator;
            ValidType newValidity;

            if (urs) newResult = rs;
            else newResult = _screenData.Result;

            if (ucbo) newCurrBinaryOperator = cbo;
            else newCurrBinaryOperator = _screenData.BinaryOperator;

            if(uvd) newValidity = vd;
            else newValidity = _screenData.Validity;

            Screen newScreenData = new Screen(newResult, newCurrBinaryOperator, newValidity);
            ScreenData = newScreenData;
        }

        private void _setInvalidOperation(ValidType validType)
        {
            _updateScreenData(false, false, true, null, null, validType);
        }

        private ValidType _validType(double num)
        {
            if (Double.IsNaN(num)) return ValidType.NAN;
            if (Double.IsInfinity(num)) return ValidType.INFINITY;
            if (num > 99999999999999 || num < -99999999999999) return ValidType.OVERFLOW;

            return ValidType.VALID;
        }
    }
}
