using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        event Action<System.Drawing.Point, System.Drawing.Point> OnMouseClick;
    }
}
