using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeCore.Extensions
{
    public static class MatrixExtensions
    {
        public static void DecomposeMatrix(this Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;
            
            matrix.Decompose(out scale3, out rotationQ, out position3);

            rotation = 0.0f;
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }
    }
}
