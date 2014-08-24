using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controllers
{
    public interface IStackedCommandsController<TControl> : IController<TControl>
    {
        void Undo();
        void Redo();

        int CommandCount { get; }
        int? CommandIndex { get; }
        bool CanUndo { get; }
        bool CanRedo { get; }
    }
}
