using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Input.Time;

namespace StoryTimeCore.Physics
{
    public interface IPhysicalWorld : IParticleBodyFactory
    {
        Vector2 Gravity { get; set; }
        void Update(WorldTime WTime);

        IBody CreateRectangularBody(float width, float height, float density, object userData = null);
    }
}
