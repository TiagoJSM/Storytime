using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Entities;
using StoryTimeCore.Input.Time;
using StoryTimeCore.Extensions;

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
            get { return BoundingBox.GetAABoundingBox(); }
        }

        public override void TimeElapse(WorldTime WTime)
        {
        }

        public override BoundingBox2D BoundingBox
        {
            get 
            {
                return
                    new BoundingBox2D(
                        Vector2.Zero,
                        new Vector2(1, 0),
                        new Vector2(1, 1),
                        new Vector2(0, 1));
            }
        }
    }
}
