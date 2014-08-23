using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using StoryTimeFramework.WorldManagement;

namespace StoryTimeDevKit.Extensions
{
    public static class SceneExtensions
    {
        public static Vector2 GetPointInGameWorld(
            this Scene scene,
            System.Drawing.Point pointInGamePanel,
            System.Drawing.Point gamePanelDimensions)
        {
            float x = scene.Camera.Viewport.Width * pointInGamePanel.X / gamePanelDimensions.X;
            float y = scene.Camera.Viewport.Height * pointInGamePanel.Y / gamePanelDimensions.Y;
            Vector2 position = new Vector2(x, y);
            return position;
        }
    }
}
