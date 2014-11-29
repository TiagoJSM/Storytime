using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeCore.Entities;

namespace StoryTimeFramework.Extensions
{
    public static class CameraExtensions
    {
        public static Vector2 GetPointInGameWorld(this ICamera camera, Point pointInScreen, Point gameScreenDimensions)
        {
            float x = camera.Viewport.Width * pointInScreen.X / gameScreenDimensions.X;
            float y = camera.Viewport.Height * pointInScreen.Y / gameScreenDimensions.Y;
            return new Vector2(x, y);
        }
    }
}
