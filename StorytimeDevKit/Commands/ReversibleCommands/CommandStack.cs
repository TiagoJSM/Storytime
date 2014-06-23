using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Commands.ReversibleCommands
{
    public class CommandStack
    {
        private List<IReversibleCommand> _commands;
        private int? _commandIndex;

        public CommandStack()
        {
            _commands = new List<IReversibleCommand>(25);
        }

        public void Push(IReversibleCommand command)
        {
            command.Run();
            if (HasUndoneItems())
            {
                RemoveUndoneItems();
                _commands.Add(command);
                _commandIndex = _commands.Count - 1;
                return;
            }

            if (_commandIndex == null)
                _commandIndex = 0;
            else
                _commandIndex++;
            _commands.Add(command);
        }

        private void RemoveUndoneItems()
        {
            if (!HasUndoneItems())
                return;
            int range = 0;
            if (_commandIndex.Value != null)
                range = _commandIndex.Value + 1;
            _commands = _commands.GetRange(0, range);
        }

        private bool HasUndoneItems()
        {
            if (_commands.Count == 0)
                return false;
            if (_commandIndex == null)
                return true;
            if (_commandIndex.Value != _commands.Count - 1)
                return true;
            return false;
        }

        //public IReversibleCommand Pop()
        //{
        //    IReversibleCommand command = _commands[_commands.Count - 1];
        //    command.Rollback();
        //    _commands.RemoveAt(_commands.Count - 1);
        //    return command;
        //}

        public void Undo()
        {
            if (_commandIndex == null)
                return;
            _commands[_commandIndex.Value].Rollback();
            if (_commandIndex == 0)
                _commandIndex = null;
            else
                _commandIndex--;
        }

        public void Redo()
        {
            if (_commandIndex == null)
                _commandIndex = 0;
            else
                _commandIndex++;
            _commands[_commandIndex.Value].Run();
        }

        public int CommandCount
        {
            get { return _commands.Count; }
        }

        public int? CommandIndex
        {
            get { return _commandIndex; }
        }

        public bool CanUndo
        {
            get
            {
                return _commandIndex != null;
            }
        }

        public bool CanRedo
        {
            get
            {
                if(_commands.Count == 0)
                    return false;
                if(_commandIndex == null)
                    return true;
                if (_commands.Count - 1 == _commandIndex.Value)
                    return false;
                return true;
            }
        }
    }
}
