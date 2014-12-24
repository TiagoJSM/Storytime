using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.Controllers
{
    public class WorkingMode
    {
        public virtual void OnEnterMode() { }
        public virtual void OnLeaveMode() { }

        public virtual void MouseMove(Vector2 positon) { }
        public virtual void MouseUp(Vector2 positon) { }
        public virtual void MouseDown(Vector2 positon) { }
        public virtual void Click(Vector2 positon) { }
    }
}
