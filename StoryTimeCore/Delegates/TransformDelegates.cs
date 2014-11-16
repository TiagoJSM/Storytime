using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;

namespace StoryTimeCore.Delegates
{
    public delegate void OnBoundingBoxChanges(IRenderableAsset renderableAsset);
    public delegate void OnRotationChanges(IRenderableAsset renderableAsset);
    public delegate void OnPositionChanges(IRenderableAsset renderableAsset);
}
