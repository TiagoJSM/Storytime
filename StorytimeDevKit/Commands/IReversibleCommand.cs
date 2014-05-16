using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Commands
{
    public interface IReversibleCommand
    {
        void Run();
        void Rollback();
    }
}
