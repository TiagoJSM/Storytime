using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StoryTimeDevKit.Commands.UICommands
{
    public class RelayCommand : BaseCommand
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

        public override bool CanExecute(object parameter)
        {
            return _f(parameter);
        }

        public override void Execute(object parameter)
        {
            _a(parameter);
        }
    }
}
