using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controllers.TemplateControllers;
using StoryTimeDevKit.Controls.Puppeteer;
using StoryTimeDevKit.Models.SceneViewer;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public class PuppeteerController : StackedCommandsController<IPuppeteerEditorControl>, IPuppeteerController
    {
        private SceneTabViewModel _sceneViewModel;

        public PuppeteerController(SceneTabViewModel s)
        {
            _sceneViewModel = s;
        }
    }
}
