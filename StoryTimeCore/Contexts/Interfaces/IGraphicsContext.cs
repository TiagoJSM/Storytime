using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeCore.DataStructures;

namespace StoryTimeCore.Contexts.Interfaces
{
    /// <summary>
    /// The interface that represents the Graphics Renderer of the engine.
    /// This interface is used to render the resources to screen and to set graphic related properties.
    /// </summary>
    public interface IRenderer
    {
        void PreRender();
        void PostRender();
        float RotationTransformation { get; set; }
        Vector2 TranslationTransformation { get; set; }
        void Render(ITexture2D texture, float x, float y, Vector2 origin = default(Vector2));
        void Render(ITexture2D texture, float x, float y, float width, float height, float rotation, Vector2 origin = default(Vector2), Vector2 renderingOffset = default(Vector2));
        void RenderRectangle(Rectangle rec, Color color, float thickness = 1.0f);
        void RenderBoundingBox(BoundingBox2D box, Color color, float thickness = 1.0f);
    }

    /// <summary>
    /// The interface that represents the Graphics Context of the engine.
    /// This interface is used to load graphic resources and obtain the renderer for the screen rendering operations.
    /// </summary>
    public interface IGraphicsContext
    {
        IRenderer GetRenderer();
        ITexture2D LoadTexture2D(string relativePath);
        ITexture2D CreateTexture2D(Color[] data, int width, int height, string name);
        void Clear(Color color);
        void SetSceneDimensions(int width, int height);
    }
}
