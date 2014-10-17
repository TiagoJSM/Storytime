using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.Actors;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public class AddBoneMode : IPuppeteerWorkingMode
    {
        private IPuppeteerWorkingModeContext _context;

        public AddBoneMode(IPuppeteerWorkingModeContext context)
        {
            _context = context;
        }

        public void Click(Vector2 position)
        {
            BoneActor bone;
            if (_context.SelectedBone == null)
            {
                bone = _context.AddBoneActor(position);
            }
            else
            {
                bone = _context.AddBoneActor(_context.SelectedBone.BoneEnd, position);
                _context.SelectedBone.AddChildren(bone);
            }

            _context.SelectedBone = bone;
        }

        public void Reset()
        {
        }
    }
}
