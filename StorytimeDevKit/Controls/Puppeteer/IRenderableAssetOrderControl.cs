using StoryTimeDevKit.Delegates.Puppeteer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public interface IRenderableAssetOrderControl
    {
        event OnAssetOrderChange OnAssetOrderChange;
    }
}
