using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace StoryTimeFramework.Entities.Interfaces
{
    public interface ICamera
    {
        Viewport Viewport { get; set; }
    }
}
