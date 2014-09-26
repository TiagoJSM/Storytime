using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeCore.DataStructures
{
    public struct Circle
    {
        public Vector2 Center;
        public float Radius;

        public bool Contains(Vector2 point)
        {
            return (point.X - Center.X).PowerOf(2) + (point.Y - Center.Y).PowerOf(2) < Radius.PowerOf(2);
        }
    }
}
