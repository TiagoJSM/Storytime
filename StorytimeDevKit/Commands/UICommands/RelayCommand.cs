using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StoryTimeDevKit.Commands.UICommands
{
    public class RelayCommand : ICommand
    {
        Action<object> _a;
        Func<object, bool> _f;

        public RelayCommand(Action<object> a)
            : this(a, obj=>true)
        {
        }

        public RelayCommand(Action<object> a, Func<object, bool> f)
        {
            _a = a;
            _f = f;
        }

        public bool CanExecute(object parameter)
        {
            return _f(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _a(parameter);
        }
    }
}
