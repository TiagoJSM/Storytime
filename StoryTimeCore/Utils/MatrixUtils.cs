using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;

namespace StoryTimeCore.Utils
{
    public static class MatrixUtils
    {
        public static Matrix CreateLocalTransformation(Vector2 position, float rotation, Vector2 scale)
        {
            var v = scale.ToVector3();
            return
                Matrix.CreateScale(scale.ToVector3()) *
                Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) *
                Matrix.CreateTranslation(position.ToVector3());
        }
    }
}
