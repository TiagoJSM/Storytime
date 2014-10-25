using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeDevKit.Controls.Puppeteer
{
    public enum PuppeteerWorkingMode
    {
        SelectionMode,
        AddBoneMode,
        Test
    }

    public interface IPuppeteerEditorControl : IMouseInteractiveControl
    {
        event Action<IPuppeteerEditorControl> OnLoaded;
        event Action<IPuppeteerEditorControl> OnUnloaded;
        event Action<PuppeteerWorkingMode> OnWorkingModeChanges;
    }
}
