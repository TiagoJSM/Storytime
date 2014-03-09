using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeFramework.Entities.Interfaces;
using Microsoft.Xna.Framework.Graphics;

namespace StoryTimeFramework.Entities.Actors
{
    public class Camera : WorldEntity, ICamera
    {
        public Viewport Viewport { get; set; }
    }
}
