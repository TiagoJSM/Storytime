using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeCore.Resources.Graphic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using StoryTimeFramework.DataStructures;
using Microsoft.Xna.Framework;

namespace StoryTime.Contexts
{
    public class XNAGraphicsContext : IGraphicsContext
    {
        private class XNARenderer : IRenderer
        {
            XNAGraphicsContext _xnaGD;

            public XNARenderer(XNAGraphicsContext xnaGD)
            {
                _xnaGD = xnaGD;
            }

            public void PreRender()
            {
                float scaleWidth = (float)_xnaGD._gdm.PreferredBackBufferWidth / (float)_xnaGD._sceneWidth;
                float scaleHeight = (float)_xnaGD._gdm.PreferredBackBufferHeight / (float)_xnaGD._sceneHeight;
                Matrix m = Matrix.CreateScale(scaleWidth, -scaleHeight, 1);
                m *= Matrix.CreateTranslation(0.0f, _xnaGD._sceneHeight, 0.0f);
                
                _xnaGD._spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.LinearClamp,
                    DepthStencilState.None,
                    RasterizerState.CullClockwise,
                    null,
                    m);
            }

            public void PostRender()
            {
                _xnaGD._spriteBatch.End();
            }

            public float RotationTransformation { get; set; }

            public Vector2 TranslationTransformation { get; set; }

            public void Render(ITexture2D texture, float x, float y, Vector2 origin = default(Vector2))
            {
                Render(texture, x, y, texture.Width, texture.Height, 0, origin);
            }

            public void Render(ITexture2D texture, float x, float y, float width, float height, float rotation, Vector2 origin = default(Vector2))
            {
                Texture2D tex = _xnaGD._gContentManager.GetTexture(texture);
                x += TranslationTransformation.X;
                y += TranslationTransformation.Y;

                rotation += RotationTransformation;

                Rectangle rec = new Rectangle(
                    (int)x,
                    (int)y, 
                    (int)width, 
                    (int)height
                );

                _xnaGD._spriteBatch.Draw(
                    tex,
                    rec, 
                    null,
                    Color.White,
                    MathHelper.ToRadians(rotation),//rotation,
                    origin,
                    SpriteEffects.FlipVertically,
                    0
                );
            }
        }

        private class XNATexture2D : ITexture2D
        {
            private int _id;
            private int _width; 
            private int _height;
            private string _texName;

            public XNATexture2D(int id, int width, int height, string textureName)
            {
                _id = id;
                _width = width;
                _height = height;
                _texName = textureName;
            }

            public int Id
            {
                get { return _id; }
            }

            public int Width
            {
                get { return _width; }
            }

            public int Height
            {
                get { return _height; }
            }

            public string TextureName
            {
                get { return _texName; }
            }

            public override int GetHashCode()
            {
                return _id;
            }

            public override bool Equals(object obj)
            {
                XNATexture2D tex = obj as XNATexture2D;
                if (tex == null)
                    return false;
                if (this._id == tex.Id)
                    return true;
                return false;
            }
        }

        private class GraphicsContentManager
        {
            private Dictionary<XNATexture2D, Texture2D> _textureContainer;
            private int _currentId;

            public GraphicsContentManager()
            {
                _textureContainer = new Dictionary<XNATexture2D, Texture2D>();
            }

            public XNATexture2D GetTextureById(int Id)
            {
                return _textureContainer.Keys.Where( 
                        (tex) => tex.Id == Id
                    )
                    .FirstOrDefault();
            }

            public XNATexture2D GetTextureByName(string name)
            {
                return _textureContainer.Keys.Where(
                        (tex) => tex.TextureName == name
                    )
                    .FirstOrDefault();
            }

            public Texture2D GetTexture(ITexture2D xnaTex)
            {
                KeyValuePair<XNATexture2D, Texture2D> pair =
                    _textureContainer.Where((kvp) => kvp.Key.Id == xnaTex.Id)
                    .FirstOrDefault();
                
                if (pair.Value == null)
                    return null;
                return pair.Value;
            }

            public XNATexture2D StoreTexture(Texture2D tex)
            {
                int validTextureId = GetValidTextureId();
                XNATexture2D xnaTex = new XNATexture2D(validTextureId, tex.Width, tex.Height, tex.Name);
                _textureContainer.Add(xnaTex, tex);
                return xnaTex;
            }

            private int GetValidTextureId()
            {
                while (GetTextureById(_currentId) != null)
                {
                    _currentId++;
                }
                _currentId++;
                return _currentId - 1;
            }
        }

        private GraphicsDevice _gd;
        private GraphicsDeviceManager _gdm;
        private SpriteBatch _spriteBatch;
        private ContentManager _cm;

        private XNARenderer _renderer;
        private GraphicsContentManager _gContentManager;

        private int _sceneWidth;
        private int _sceneHeight;

        public GraphicsDevice GraphicsDevice { get { return _gd; } }
        public GraphicsDeviceManager GraphicsDeviceManager { get { return _gdm; } }
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public ContentManager ContentManager { get { return _cm; } }
        public int SceneWidth { get { return _sceneWidth; } }
        public int SceneHeight { get { return _sceneHeight; } }

        public XNAGraphicsContext(GraphicsDeviceManager gdm, ContentManager cm)
        {
            _gdm = gdm;
            _gd = gdm.GraphicsDevice;
            _spriteBatch = new SpriteBatch(_gd);
            _cm = cm;
            _gContentManager = new GraphicsContentManager();
            _renderer = new XNARenderer(this);
            
            _sceneWidth = 720;
            _sceneHeight = 480;

            _gdm.PreferredBackBufferWidth = 1280;
            _gdm.PreferredBackBufferHeight = 720;
            _gdm.ApplyChanges();
        }

        public IRenderer GetRenderer()
        {
            return _renderer;
        }

        public ITexture2D LoadTexture2D(string relativePath)
        {
            XNATexture2D tex = _gContentManager.GetTextureByName(relativePath);
            if (tex != null) return tex;
            Texture2D t2d = _cm.Load<Texture2D>(relativePath);
            return _gContentManager.StoreTexture(t2d);
        }

        public ITexture2D CreateTexture2D(Color[] data, int width, int height, string name)
        {
            XNATexture2D tex = _gContentManager.GetTextureByName(name);
            if (tex != null) return tex;
            Texture2D t2d = new Texture2D(_gd, width, height);
            t2d.SetData(data);
            t2d.Name = name;
            return _gContentManager.StoreTexture(t2d);
        }

        public void Clear(Color color)
        {
            _gd.Clear(color);
        }

        public void SetSceneDimensions(int width, int height)
        {
            _sceneWidth = width;
            _sceneHeight = height;
        }

        public void SetBackBufferDimensions(int width, int height) 
        {
            _gdm.PreferredBackBufferWidth = width;
            _gdm.PreferredBackBufferHeight = height;
            _gdm.ApplyChanges();
        }
    }
}
