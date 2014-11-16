using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeDevKit.Entities.Actors;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public class AddBoneMode : PuppeteerWorkingMode
    {
        private IPuppeteerWorkingModeContext _context;

        public AddBoneMode(IPuppeteerWorkingModeContext context)
        {
            _context = context;
        }

        public override void OnEnterMode()
        {
        }

        public override void OnLeaveMode()
        {
        }

        public override void Click(Vector2 position)
        {
            BoneActor bone;
            BoneActor selectedBone = _context.Selected as BoneActor;
            if (selectedBone == null)
            {
                bone = _context.AddBone(position);
            }
            else
            {
                bone = _context.AddBone(selectedBone.BoneEnd, position);
            }

            _context.Selected = bone;
        }
    }
}
