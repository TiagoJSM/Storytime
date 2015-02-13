using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace StoryTimeCore.General
{
    public interface IPositionable
    {
        Vector2 Origin { get; set; }
        Vector2 RenderingOffset { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
    }
}
