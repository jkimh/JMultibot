using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace JClientBot.Commons
{
    public class Command : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public Command(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this._execute = executeMethod;
            this._canExecute = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
