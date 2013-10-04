using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MvvmDemo
{
    class DelegateCommand<T> : ICommand
    {
        private Action<T> execute_;
        private Predicate<T> canExecute_;

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            execute_ = execute;
            canExecute_ = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (canExecute_ == null)
            {
                return true;
            }
            else
            {
                return canExecute_((T)parameter);
            }
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void Execute(object parameter)
        {
            execute_((T)parameter);
        }

        public void RaiseChangeExecute()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }
    }
}
