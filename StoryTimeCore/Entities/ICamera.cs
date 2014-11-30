using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StoryTimeCore.Entities
{
    public interface ICamera
    {
        Matrix ViewMatrix { get; }
        Matrix ProjectionMatrix { get; }
        Viewport Viewport { get; set; }
    }
}
