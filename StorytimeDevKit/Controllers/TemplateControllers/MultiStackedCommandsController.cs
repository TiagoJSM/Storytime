using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Commands.ReversibleCommands;

namespace StoryTimeDevKit.Controllers.TemplateControllers
{
    public class MultiStackedCommandsController<TControl, TKey> : IMultiStackedCommandsController<TControl, TKey> where TKey: class
    {
        private Dictionary<TKey, CommandStack> _commands;
        private TKey _stackKey;
        
        protected CommandStack SelectedStack { get; private set; }

        public TKey StackKey
        {
            get
            {
                return _stackKey;
            }
            set
            {
                if(_stackKey == value) return;
                if (!_commands.ContainsKey(value)) return;
                _stackKey = value;
                SelectedStack = _commands[value];
            }
        }

        public int CommandCount
        {
            get 
            {
                if (SelectedStack == null) return 0;
                return SelectedStack.CommandCount;
            }
        }

        public int? CommandIndex
        {
            get 
            {
                if (SelectedStack == null) return null;
                return SelectedStack.CommandIndex;
            }
        }

        public bool CanUndo
        {
            get 
            {
                if (SelectedStack == null) return false;
                return SelectedStack.CanUndo;
            }
        }

        public bool CanRedo
        {
            get
            {
                if (SelectedStack == null) return false;
                return SelectedStack.CanRedo;
            }
        }

        public MultiStackedCommandsController()
        {
            _commands = new Dictionary<TKey, CommandStack>();
        }

        public void Undo()
        {
            if (SelectedStack != null)
                SelectedStack.Undo();
        }

        public void Redo()
        {
            if (SelectedStack != null)
                SelectedStack.Redo();
        }

        public void AddStackFor(TKey key)
        {
            _commands.Add(key, new CommandStack());
        }

        public void RemoveStackFor(TKey key)
        {
            _commands.Remove(key);
        }
    }
}
