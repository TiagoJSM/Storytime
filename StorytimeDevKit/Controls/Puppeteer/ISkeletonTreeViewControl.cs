using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public interface ISkeletonTreeViewControl : IControl
    {
        event Action<ISkeletonTreeViewControl> OnLoaded;
        event Action<ISkeletonTreeViewControl> OnUnloaded;
    }
}
