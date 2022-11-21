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
        
        public Screen ScreenData
        {
            get { return _screenData; }
            set 
            { 
                _screenData = value;

                Console.WriteLine(_calculator.State.FirstNumber);

                OnPropertyChanged(nameof(ScreenData));
            }
        }

        private ICommand _buttonCommand;
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
                        OperandHandler(op);
                        break;
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        BinaryOperatorHandler(op);
                        break;
                    case "1/x":
                    case "sqrt":
                    case "square":
                    case "+/-":
                    case "%":                       
                        UnaryOperatorHandler(op);
                        break;
                    case "C":
                    case "CE":
                    case "=":
                    case ".":
                    case "Back":
                        FunctionalButtonHandler(op);
                        break;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());

                FunctionalButtonHandler("C");
                SetInvalid(ValidityType.EXCEPTION);
            }                     
        }

        /* The following are button handlers */

        private void OperandHandler(string op) 
        {
            /* Currently handling the first number */
            if (_calculator.State.BinaryOperator == null && _calculator.State.LastNumber == null)  
            {
                string newFirstNumber;
                /* The first number == 0 */
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
                if (ValidityType_(Convert.ToDouble(newFirstNumber)) != ValidityType.VALID)
                {
                    SetInvalid(ValidityType_(Convert.ToDouble(newFirstNumber)));
                }
                /* Valid => Update the state */
                else
                {
                    UpdateCalculator(true, false, false, newFirstNumber, null, null);
                    UpdateScreenData(true, false, true, newFirstNumber, null, ValidityType.VALID);                    
                }            
            }
            /* Currently handling the last number */
            else
            {
                string newLastNumber;
                /* The last number == 0 */
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
                if (ValidityType_(Convert.ToDouble(newLastNumber)) != ValidityType.VALID)
                {
                    SetInvalid(ValidityType_(Convert.ToDouble(newLastNumber)));
                }
                /* Valid => Update the state */
                else
                {
                    UpdateCalculator(false, false, true, null, null, newLastNumber);
                    UpdateScreenData(true, false, true, newLastNumber, null, ValidityType.VALID);
                }
                    
            }
        }
        private void UnaryOperatorHandler(string op) 
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
                if (ValidityType_(firstNumber) != ValidityType.VALID) 
                {
                    SetInvalid(ValidityType_(firstNumber));
                }
                /* Valid => Update the state */
                else
                {
                    string newFirstNumber = firstNumber.ToString();
                    UpdateCalculator(true, false, false, newFirstNumber, null, null);
                    UpdateScreenData(true, false, true, newFirstNumber, null, ValidityType.VALID);
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
                if (ValidityType_(lastNumber) != ValidityType.VALID) 
                {   
                    SetInvalid(ValidityType_(lastNumber));
                }
                /* Valid => Update the state */
                else
                {
                    string newLastNumber = lastNumber.ToString();
                    UpdateCalculator(false, false, true, null, null, newLastNumber);
                    UpdateScreenData(true, false, true, newLastNumber, null, ValidityType.VALID);
                }                   
            }
            /* Currently existing a binary operator  */
            else
            {
                SetInvalid(ValidityType.INVALID_OPERATION);
            }
        }
        private void BinaryOperatorHandler(string op) 
        {
            /* The last number does not currently exist */
            if (_calculator.State.LastNumber == null)
            {
                UpdateCalculator(false, true, false, null, op, null);
                UpdateScreenData(false, true, false, null, op, ValidityType.VALID);
            }
            /* The last number currently exist, calculate the current expression */
            else
            {
                /* Divide by 0 */
                if (_calculator.State.BinaryOperator == "/" && _calculator.State.LastNumber == "0") 
                {
                    SetInvalid(ValidityType.DIVIDE_BY_ZERO);
                    return;
                }
                
                
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
                if (ValidityType_(res) != ValidityType.VALID) // something wrong
                {
                    SetInvalid(ValidityType_(res));
                }
                /* Valid => Update the state */
                else
                {
                    string newFirstNumber = res.ToString();
                    UpdateCalculator(true, true, true, newFirstNumber, op, null);
                    UpdateScreenData(true, true, true, newFirstNumber, op, ValidityType.VALID);
                }               
            }
        }
        private void FunctionalButtonHandler(string op) 
        {
            switch (op)
            {
                #region "C" Handler
                /* Clear all */
                case "C":
                    UpdateCalculator(true, true, true, "0", null, null);
                    UpdateScreenData(true, true, true, "0", null, ValidityType.VALID);
                    break;
                #endregion
                #region "CE" Handler
                /* Clear error */
                case "CE":
                    /* Currently handling the first number */
                    if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        UpdateCalculator(false, true, true, null, null, null);
                        UpdateScreenData(false, true, true, null, null, ValidityType.VALID);
                    }
                    /* Currently handling the last number */
                    else if (_calculator.State.LastNumber != null)
                    {
                        UpdateCalculator(false, false, true, null, null, null);
                        UpdateScreenData(true, false, true, null, null, ValidityType.VALID);                       
                    }
                    break;
                #endregion
                #region "=" Handler
                    /* Calulate the current expression */
                case "=":
                    /* The last number does not currently exist */
                    if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        SetInvalid(ValidityType.INVALID_OPERATION);
                    }
                    else
                    {
                        BinaryOperatorHandler(null);
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
                        UpdateCalculator(true, false, false, newFirstNumber, null, null);
                        UpdateScreenData(true, false, true, newFirstNumber, null, ValidityType.VALID);
                    }
                    /* Currently handling the last number and decimal point does not exist  */
                    else if (_calculator.State.LastNumber != null
                        && !_calculator.State.LastNumber.Contains('.'))
                    {
                        string newLastNumber = _calculator.State.LastNumber + ".";
                        UpdateCalculator(false, false, true, null, null, newLastNumber);
                        UpdateScreenData(true, false, true, newLastNumber, null, ValidityType.VALID);
                    }
                    /* Other cases are all invalid */
                    else
                    {
                        SetInvalid(ValidityType.INVALID_OPERATION);
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
                            UpdateCalculator(true, false, false, "0", null, null);
                            UpdateScreenData(true, false, true, "0", null, ValidityType.VALID);
                        }
                        else
                        {
                            string newFirstNumber = _calculator.State.FirstNumber.Remove(_calculator.State.FirstNumber.Length - 1);
                            if(newFirstNumber == "-")
                            {
                                UpdateCalculator(true, false, false, "0", null, null);
                                UpdateScreenData(true, false, true, "0", null, ValidityType.VALID);
                            }
                            else
                            {
                                UpdateCalculator(true, false, false, newFirstNumber, null, null);
                                UpdateScreenData(true, false, true, newFirstNumber, null, ValidityType.VALID);
                            }                           
                        } 
                    }
                    /* Currently handling the binary operator */
                    else if (_calculator.State.BinaryOperator != null && _calculator.State.LastNumber == null)
                    {
                        UpdateCalculator(false, true, false, null, null, null);
                        UpdateScreenData(false, true, true, null, null, ValidityType.VALID);                  
                    }
                    /* Currently handling the last number */
                    else
                    {
                        if (_calculator.State.LastNumber.Length <= 1)
                        {
                            UpdateCalculator(false, false, true, null, null, "0");
                            UpdateScreenData(true, false, true, "0", null, ValidityType.VALID);
                        }
                        else
                        {
                            string newLastNumber = _calculator.State.LastNumber.Remove(_calculator.State.LastNumber.Length - 1);
                            if(newLastNumber == "-")
                            {
                                UpdateCalculator(false, false, true, null, null, "0");
                                UpdateScreenData(true, false, true, "0", null, ValidityType.VALID);
                            }
                            else
                            {
                                UpdateCalculator(false, false, true, null, null, newLastNumber);
                                UpdateScreenData(true, false, true, newLastNumber, null, ValidityType.VALID);
                            }                           
                        }
                    }
                    break;
                #endregion
            }
        }
        
        /* The following are some helper functions */
        private void UpdateCalculator(bool ufn, bool ubo, bool uln, string fn, string bo, string ln) 
        {           
            string newFirstNumber;
            string newBinaryOperator;
            string newLastNumber;

            if (ufn) newFirstNumber = fn;
            else newFirstNumber = _calculator.State.FirstNumber;

            if (ubo) newBinaryOperator = bo;
            else newBinaryOperator = _calculator.State.BinaryOperator;

            if (uln) newLastNumber = ln;
            else newLastNumber= _calculator.State.LastNumber;
          
            CalculatorState newState = new(newFirstNumber, newBinaryOperator, newLastNumber);
            Calculator newCalculator = new(newState);
            _calculator = newCalculator;           
        }
        private void UpdateScreenData(bool urs, bool ubo, bool uvd, string rs, string bo, ValidityType vd) 
        {
            string newResult;
            string newBinaryOperator;
            ValidityType newValidity;

            if (urs) newResult = rs;
            else newResult = _screenData.Result;

            if (ubo) newBinaryOperator = bo;
            else newBinaryOperator = _screenData.BinaryOperator;

            if(uvd) newValidity = vd;
            else newValidity = _screenData.Validity;

            Screen newScreenData = new(newResult, newBinaryOperator, newValidity);
            ScreenData = newScreenData;
        }

        private void SetInvalid(ValidityType validityType)
        {
            UpdateScreenData(false, false, true, null, null, validityType);
        }

        private static ValidityType ValidityType_(double num)
        {
            if (Double.IsNaN(num)) return ValidityType.NAN;           
            else if (num > Double.MaxValue || num < Double.MinValue) return ValidityType.OVERFLOW;                   
            else return ValidityType.VALID;
        }
    }
}
