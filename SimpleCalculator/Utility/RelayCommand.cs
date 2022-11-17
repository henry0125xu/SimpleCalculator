using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCalculator.Utility
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }
        
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public event EventHandler CanExecuteChanged;
        public void Execute(object parameter)
        {
            if(parameter != null)
            {
                _execute(parameter);
            }
            else
            {
                _execute("Hello");
            }
        }   
    }
}
