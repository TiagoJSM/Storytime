using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StoryTimeCore.Entities;

namespace StoryTimeFramework.Entities.Actors
{
    public class Camera : WorldEntity, ICamera
    {
        public Matrix ViewMatrix
        {
            get
            {
                var origin = new Vector3(-Viewport.Width/2, -Viewport.Height/2, 0);
                return Matrix.CreateTranslation(origin + new Vector3(Viewport.X, Viewport.Y, 0));
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                return Matrix.CreateOrthographic(Viewport.Width, Viewport.Height, -100000, 100000);
            }
        }

        public Viewport Viewport { get; set; }
    }
}
