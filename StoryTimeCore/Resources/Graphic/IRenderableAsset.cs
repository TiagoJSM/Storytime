using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using StoryTimeCore.Manageables;
using StoryTimeCore.General;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;
using StoryTimeCore.Delegates;

namespace StoryTimeCore.Resources.Graphic
{
    /// <summary>
    /// The interface that defines a renderable asset.
    /// The ITimeUpdatable interface is inherited due to possible animations by the asset that are time dependent.
    /// </summary>
    public interface IRenderableAsset : IRenderable, ITimeUpdatable, IBoundingBoxable
    {
        event OnBoundingBoxChanges OnBoundingBoxChanges;
        event OnRotationChanges OnRotationChanges;
        event OnPositionChanges OnPositionChanges;

        bool IsVisible { get; set; }
        Vector2 Origin { get; set; }
        Vector2 RenderingOffset { get; set; }
        float Rotation { get; set; }
        Vector2 Scale { get; set; }
        AxisAlignedBoundingBox2D BoundingBoxWithoutOrigin { get; }
    }
}
