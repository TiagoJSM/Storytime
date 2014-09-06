using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.DataStructures;
using StoryTimeFramework.Resources.Graphic;
using Microsoft.Xna.Framework;

namespace StoryTimeDevKit.SceneWidgets.Interfaces.Puppeteer
{
    public class BoneRenderableAsset : Static2DRenderableAsset
    {
        public BoneRenderableAsset(IGraphicsContext graphicsContext)
        {
            Texture2D = graphicsContext.LoadTexture2D("ArrowVertical");
            Origin = new Vector2(Texture2D.Width / 2, 0.0f);
        }
    }
}
