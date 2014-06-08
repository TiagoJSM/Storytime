using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controllers
{
    public interface IController 
    { 
    }

    public interface IController<TControl> : IController
    {
    }
}
