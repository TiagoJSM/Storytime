using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Delegates.Puppeteer;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public enum PuppeteerWorkingMode
    {
        SelectionMode,
        AddBoneMode
    }

    public interface IPuppeteerEditorControl
    {
        event Action<IPuppeteerEditorControl> OnLoaded;
        event Action<IPuppeteerEditorControl> OnUnloaded;
        event Action<PuppeteerWorkingMode> OnWorkingModeChanges;
        event OnMouseClick OnMouseClick;
    }
}
