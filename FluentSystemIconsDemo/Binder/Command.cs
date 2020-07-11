using System;
using System.Windows.Input;

namespace FluentSystemIconsDemo.Binder
{
    public class Command : ICommand
    {
        readonly Action<object> action;
        readonly Func<object, bool> canAction;

        public event EventHandler CanExecuteChanged;

        public Command(Action<object> action, Func<object, bool> canAction = null)
        {
            this.action = action;
            this.canAction = canAction;
        }

        public bool CanExecute(object parameter)
        {
            return canAction?.Invoke(parameter) != false;
        }

        public void Execute(object parameter)
        {
            action?.Invoke(parameter);
        }

        public void OnChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}