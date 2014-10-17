﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public class SelectionMode : IPuppeteerWorkingMode
    {
        private IPuppeteerWorkingModeContext _context;

        public SelectionMode(IPuppeteerWorkingModeContext context)
        {
            _context = context;
        }

        public void Click(Vector2 position)
        {
            _context.SelectedBone = _context.GetIntersectedBone(position);
        }

        public void Reset()
        {
        }
    }
}
