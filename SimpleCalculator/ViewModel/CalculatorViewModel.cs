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

    internal class CalculatorViewModel : ViewModelBase
    {
        private Calculator _calculator;
        //private string _result;
        //private string _currBinaryOperator;
        //private bool _inValidOperation;
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

        /*
        public string Result 
        {
            get { return _result; } 
            set 
            { 
                _result = value;
                OnPropertyChanged("Result");
            }
        }
        public string CurrBinaryOperator
        {
            get { return _currBinaryOperator; }
            set
            {
                _currBinaryOperator = value;
                OnPropertyChanged("CurrBinaryOperator");
            }
        }
        public bool InValidOperation
        {
            get { return _inValidOperation; }
            set
            {
                _inValidOperation = value;
                OnPropertyChanged("InValidOperation");
            }
        }

        */
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
            _screenData = _calculator.ScreenData;
            //Result = _calculator.ScreenData.Result;
            //CurrBinaryOperator = _calculator.ScreenData.CurrBinaryOperator;
            //InValidOperation = _calculator.ScreenData.InValidOperation;
           
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
                case "reciprocal":
                case "sqrt":
                case "square":
                case "negative":
                case "persent":
                    _unaryOperatorHandler(bn);
                    break;
                case "clear":
                case "clearError":
                case "equal":
                case "dot":
                case "revise":
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
                string newFirstNumber = _calculator.State.FirstNumber + op;       
                newState = new CalculatorState(newFirstNumber, string.Empty, string.Empty, _calculator.State.HaveDot);
                newScreenData = new Screen(newFirstNumber, _calculator.ScreenData.CurrBinaryOperator, _calculator.ScreenData.InValidOperation);
            }
            else 
            {
                string newLastNumber = _calculator.State.LastNumber + op;
                newState = new CalculatorState(_calculator.State.FirstNumber, _calculator.State.BinaryOperator, newLastNumber, _calculator.State.HaveDot);
                newScreenData = new Screen(newLastNumber, _calculator.ScreenData.CurrBinaryOperator, _calculator.ScreenData.InValidOperation);
            }

            Calculator newCalculator = new Calculator(newState, newScreenData);
            _calculator = newCalculator;
            ScreenData = newScreenData;
            //Result = _calculator.ScreenData.Result;
            //CurrBinaryOperator = _calculator.ScreenData.CurrBinaryOperator;
            //InValidOperation = _calculator.ScreenData.InValidOperation;

            //Console.WriteLine(Result);
        }
        private void _unaryOperatorHandler(string op) { }
        private void _binaryOperatorHandler(string op) { }
        private void _functionalButtonHandler(string op) { }
    }

}
