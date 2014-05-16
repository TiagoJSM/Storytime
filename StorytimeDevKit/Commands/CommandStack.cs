using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Commands
{
    public class CommandStack
    {
        private Stack<IReversibleCommand> _commands;

        public CommandStack()
        {
            _commands = new Stack<IReversibleCommand>(25);
        }

        public void Push(IReversibleCommand command)
        {
            _commands.Push(command);
        }

        public IReversibleCommand Pop()
        {
            return _commands.Pop();
        }
    }
}
