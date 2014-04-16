using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controllers
{
    public interface IController<TControl>
    {
        TControl Control { set; }
    }
}
