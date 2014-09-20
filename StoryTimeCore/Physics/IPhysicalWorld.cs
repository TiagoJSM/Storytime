using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.Physics
{
    public interface IPhysicalWorld
    {
        Vector2 Gravity { get; set; }

        IBody CreateRectangularBody(float width, float height, float density, object userData = null);
    }
}
