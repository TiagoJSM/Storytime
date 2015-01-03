using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Entities;
using StoryTimeCore.Input.Time;

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

        public override AxisAlignedBoundingBox2D AABoundingBox
        {
            get { return new AxisAlignedBoundingBox2D(0, 0, 1); }
        }

        public override BoundingBox2D BoundingBox
        {
            get { return new BoundingBox2D(new Vector2(1)); }
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }
    }
}
