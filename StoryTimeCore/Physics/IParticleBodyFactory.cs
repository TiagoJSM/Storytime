using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StoryTimeCore.Physics
{
    public interface IParticleBodyFactory
    {
        IBody CreateParticleBody(bool physicalSimulated, float width, float height, float density);
    }
}
