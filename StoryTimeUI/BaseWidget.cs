using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework;
using StoryTimeUI.Interfaces;

namespace StoryTimeUI
{
    public abstract class BaseWidget
    {
        private IGraphicsContext _graphicsContext;
        private bool _initialized;

        public event Action OnInitialize;

        public IGraphicsContext GraphicsContext
        {  
            get 
            {
                return _graphicsContext;
            }
            set 
            {
                if(_graphicsContext == null)
                    _graphicsContext = value;
            }
    }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public IParent Parent { get; set; }

        protected abstract IRenderableAsset RenderableAsset { get; }

        public void Render(IRenderer renderer)
        {
            renderer.TranslationTransformation += Position;
            renderer.RotationTransformation += Rotation;
            RenderableAsset.Render(renderer);
            renderer.RotationTransformation -= Rotation;
            renderer.TranslationTransformation -= Position;
        }

        public void Initialize()
        {
            if (_initialized) return;
            if (OnInitialize != null)
                OnInitialize();
            _initialized = true;
        }
    }
}
