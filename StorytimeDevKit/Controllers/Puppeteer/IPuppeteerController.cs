﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeDevKit.Controls.Puppeteer;

namespace StoryTimeDevKit.Controllers.Puppeteer
{
    public interface IPuppeteerController : IStackedCommandsController<IPuppeteerEditorControl>
    {
        IPuppeteerEditorControl PuppeteerControl { get; set; }
    }
}
