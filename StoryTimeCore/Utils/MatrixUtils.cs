using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Extensions;
using StoryTimeCore.General;
using StoryTimeCore.Resources.Graphic;

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

        public static Matrix CreateRenderableAssetTransformation(IPositionable asset)
        {
            return
                Matrix.CreateTranslation(-new Vector3(asset.Origin, 0))
                * Matrix.CreateScale(new Vector3(asset.Scale, 1))
                * Matrix.CreateRotationZ(MathHelper.ToRadians(asset.Rotation))
                * Matrix.CreateTranslation(new Vector3(asset.Origin, 0))
                * Matrix.CreateTranslation(new Vector3(asset.RenderingOffset, 0));
        }
    }
}
