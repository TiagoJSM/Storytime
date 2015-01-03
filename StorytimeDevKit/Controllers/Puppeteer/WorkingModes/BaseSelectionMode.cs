using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeFramework.Entities.Actors;

namespace StoryTimeDevKit.Controllers.Puppeteer.WorkingModes
{
    public abstract class BaseSelectionMode : WorkingMode
    {
        private bool _intersectedWidget;

        protected IPuppeteerWorkingModeContext Context { get; private set; }

        public BaseSelectionMode(IPuppeteerWorkingModeContext context)
        {
            Context = context;
        }

        public override void OnEnterMode()
        {
            Context.EnableTransformationUI(true);
        }

        public override void OnLeaveMode()
        {
            Context.EnableTransformationUI(false);
        }

        public override void MouseMove(Vector2 positon) 
        {
            Context.Scene.GUI.MouseMove(positon);
        }

        public override void MouseUp(Vector2 positon) 
        {
            Context.Scene.GUI.MouseUp(positon);
        }

        public override void MouseDown(Vector2 positon) 
        {
            _intersectedWidget = Context.Scene.GUI.Intersect(positon) != null;
            Context.Scene.GUI.MouseDown(positon);
        }

        public override void Click(Vector2 position)
        {
            if (_intersectedWidget) return;
            //_context.SelectedBone = _context.GetIntersectedBone(position);
            var actor = Context.GetIntersectedActor(position);
            if(actor != null)
                HandleActorIntersection(actor, position);
        }

        protected abstract void HandleActorIntersection(BaseActor actor, Vector2 position);
    }
}
