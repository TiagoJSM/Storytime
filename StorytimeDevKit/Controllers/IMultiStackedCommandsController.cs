using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controllers
{
    public interface IMultiStackedCommandsController<TControl, TKey> : IStackedCommandsController<TControl>
    {
        TKey StackKey { get; set; }
        void AddStackFor(TKey key);
        void RemoveStackFor(TKey key);
    }
}
