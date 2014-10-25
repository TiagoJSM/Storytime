using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public interface IPuppeteerWorkingMode
    {
        void OnEnterMode();
        void OnLeaveMode();
        
        void Click(Vector2 positon);
    }
}
