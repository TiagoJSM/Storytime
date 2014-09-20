using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeDevKit.Commands.ReversibleCommands;

namespace StoryTimeDevKit.Controllers.TemplateControllers
{
    public abstract class StackedCommandsController<TControl> : IStackedCommandsController<TControl>
    {
        private CommandStack _commands;

        public StackedCommandsController()
        {
            _commands = new CommandStack();
        }

        public void Undo()
        {
            _commands.Undo();
        }

        public void Redo()
        {
            _commands.Redo();
        }

        public int CommandCount
        {
            get { return _commands.CommandCount; }
        }

        public int? CommandIndex
        {
            get { return _commands.CommandIndex; }
        }

        public bool CanUndo
        {
            get { return _commands.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _commands.CanRedo; }
        }

        protected CommandStack Commands { get { return _commands; } }
    }
}
