using System;
using System.Windows.Input;

namespace MailDownloader
{
    public class RelayCommand : ICommand
    {
        private Action _targetExecuteMethod;
        private Func<bool> _targetCanExecuteMethod;

        public RelayCommand(Action executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
                return _targetCanExecuteMethod();
            if (_targetExecuteMethod != null)
                return true;
            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_targetExecuteMethod != null)
                _targetExecuteMethod();
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private Action<T> _targetExecuteMethod;
        private Func<T, bool> _targetCanExecuteMethod;

        public RelayCommand(Action<T> executeMethod)
        {
            _targetExecuteMethod = executeMethod;
        }

        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
        {
            _targetExecuteMethod = executeMethod;
            _targetCanExecuteMethod = canExecuteMethod;
        }

        public event EventHandler CanExecuteChanged = delegate { };

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (_targetCanExecuteMethod != null)
            {
                T tparm = (T)parameter;
                return _targetCanExecuteMethod(tparm);
            }

            if (_targetExecuteMethod != null)
                return true;

            return false;
        }

        void ICommand.Execute(object parameter)
        {
            if (_targetExecuteMethod != null)
                _targetExecuteMethod((T)parameter);
        }
    }
}
