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
using StoryTimeCore.Extensions;
using C3.XNA;
using StoryTimeCore.DataStructures;
using System.IO;
using StoryTimeCore.Entities;

namespace StoryTime.Contexts
{
    public class XNAGraphicsContext : IGraphicsContext
    {
        private class XNARenderer : IRenderer
        {
            XNAGraphicsContext _xnaGD;
            VertexPositionTexture[] _vertices;

            public XNARenderer(XNAGraphicsContext xnaGD)
            {
                _xnaGD = xnaGD;
                InitializeVertices();
            }

            public void PreRender()
            {
                var scaleWidth = (float)_xnaGD._gdm.PreferredBackBufferWidth / (float)_xnaGD._cameraWidth;
                var scaleHeight = (float)_xnaGD._gdm.PreferredBackBufferHeight / (float)_xnaGD._cameraHeight;
                var m = Matrix.CreateScale(scaleWidth, -scaleHeight, 1);
                m *= Matrix.CreateTranslation(0.0f, _xnaGD._cameraHeight, 0.0f);

                var rs = new RasterizerState();
                rs.CullMode = CullMode.None;
                _xnaGD._gd.RasterizerState = rs;
                _xnaGD._basicEffect.TextureEnabled = true;

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

            public void Render(ITexture2D texture, float x, float y, float width, float height, float rotation, Vector2 origin = default(Vector2), Vector2 renderingOffset = default(Vector2))
            {
                var tex = _xnaGD._gContentManager.GetTexture(texture);
                x += TranslationTransformation.X;
                y += TranslationTransformation.Y;

                rotation += RotationTransformation;

                var rec = new Rectangle(
                    (int)(x + origin.X + renderingOffset.X),
                    (int)(y + origin.Y + renderingOffset.Y), 
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

            public void Render(ITexture2D texture, Matrix transformation, AxisAlignedBoundingBox2D boundingBox)
            {
                var tex = _xnaGD._gContentManager.GetTexture(texture);
                var worldMatrixPreTransform = _xnaGD._basicEffect.World;

                _xnaGD._basicEffect.Texture = tex;
                _xnaGD._basicEffect.World *= transformation * RendererTransformation();
                _xnaGD._basicEffect.View = _xnaGD.ViewMatrix;
                _xnaGD._basicEffect.Projection = _xnaGD.ProjectionMatrix;

                foreach (var pass in _xnaGD._basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    _xnaGD._gd.DrawUserPrimitives(PrimitiveType.TriangleStrip, GetVerticesFor(boundingBox), 0, 2);
                }
                _xnaGD._basicEffect.World = worldMatrixPreTransform;
            }

            public void RenderRectangle(Rectangle rec, Color color, float thickness = 1.0f)
            {
                _xnaGD._spriteBatch.DrawRectangle(rec, color, thickness);
            }

            public void RenderBoundingBox(BoundingBox2D box, Color color, float thickness = 1.0f)
            {
                _xnaGD._spriteBatch.DrawLine(box.Point1, box.Point2, color, thickness);
                _xnaGD._spriteBatch.DrawLine(box.Point2, box.Point3, color, thickness);
                _xnaGD._spriteBatch.DrawLine(box.Point3, box.Point4, color, thickness);
                _xnaGD._spriteBatch.DrawLine(box.Point4, box.Point1, color, thickness);
            }

            private void InitializeVertices()
            {
                _vertices = new VertexPositionTexture[4];

                _vertices[0].Position = new Vector3(0, 1, 20);
                _vertices[0].TextureCoordinate = new Vector2(0, 0);

                _vertices[1].Position = new Vector3(0, 0, 20);
                _vertices[1].TextureCoordinate = new Vector2(0, 1);

                _vertices[2].Position = new Vector3(1, 1, 20);
                _vertices[2].TextureCoordinate = new Vector2(1, 0);

                _vertices[3].Position = new Vector3(1, 0, 20);
                _vertices[3].TextureCoordinate = new Vector2(1, 1);
            }

            private Matrix RendererTransformation()
            {
                return 
                    Matrix.CreateRotationZ(MathHelper.ToRadians(RotationTransformation)) * 
                    Matrix.CreateTranslation(new Vector3(TranslationTransformation, 0));
            }

            private VertexPositionTexture[] GetVerticesFor(AxisAlignedBoundingBox2D boundingBox)
            {
                _vertices[0].Position.X = boundingBox.X;
                _vertices[0].Position.Y = boundingBox.Height;

                _vertices[1].Position.X = boundingBox.X;
                _vertices[1].Position.Y = boundingBox.Y;

                _vertices[2].Position.X = boundingBox.Width;
                _vertices[2].Position.Y = boundingBox.Height;

                _vertices[3].Position.X = boundingBox.Width;
                _vertices[3].Position.Y = boundingBox.Y;

                return _vertices;
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
                var tex = obj as XNATexture2D;
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
                var pair =
                    _textureContainer.Where((kvp) => kvp.Key.Id == xnaTex.Id)
                    .FirstOrDefault();
                
                if (pair.Value == null)
                    return null;
                return pair.Value;
            }

            public XNATexture2D StoreTexture(Texture2D tex)
            {
                var validTextureId = GetValidTextureId();
                var xnaTex = new XNATexture2D(validTextureId, tex.Width, tex.Height, tex.Name);
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

        private int _cameraWidth;
        private int _cameraHeight;

        private BasicEffect _basicEffect;

        public GraphicsDevice GraphicsDevice { get { return _gd; } }
        public GraphicsDeviceManager GraphicsDeviceManager { get { return _gdm; } }
        public SpriteBatch SpriteBatch { get { return _spriteBatch; } }
        public ContentManager ContentManager { get { return _cm; } }
        public int SceneWidth { get { return _cameraWidth; } }
        public int SceneHeight { get { return _cameraHeight; } }
        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }

        public XNAGraphicsContext(GraphicsDeviceManager gdm, ContentManager cm)
        {
            _gdm = gdm;
            _gd = gdm.GraphicsDevice;
            _spriteBatch = new SpriteBatch(_gd);
            _cm = cm;
            _gContentManager = new GraphicsContentManager();
            _renderer = new XNARenderer(this);

            _cameraWidth = 720;
            _cameraHeight = 480;

            _gdm.PreferredBackBufferWidth = 1280;
            _gdm.PreferredBackBufferHeight = 720;

            _basicEffect = new BasicEffect(_gd);
        }

        public IRenderer GetRenderer()
        {
            return _renderer;
        }

        public ITexture2D LoadTexture2D(string relativePath)
        {
            var tex = _gContentManager.GetTextureByName(relativePath);
            if (tex != null) return tex;
            var t2d = _cm.Load<Texture2D>(relativePath);
            return _gContentManager.StoreTexture(t2d);
        }

        public ITexture2D CreateTexture2D(string fullPath)
        {
            var tex = _gContentManager.GetTextureByName(fullPath);
            if (tex != null) return tex;
            Texture2D texture;
            using (Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                texture = Texture2D.FromStream(_gd, stream);
            }
            return _gContentManager.StoreTexture(texture);
        }

        public ITexture2D CreateTexture2D(Color[] data, int width, int height, string name)
        {
            var tex = _gContentManager.GetTextureByName(name);
            if (tex != null) return tex;
            var t2d = new Texture2D(_gd, width, height);
            t2d.SetData(data);
            t2d.Name = name;
            return _gContentManager.StoreTexture(t2d);
        }

        public void Clear(Color color)
        {
            _gd.Clear(color);
        }

        /*public void SetSceneDimensions(int width, int height)
        {
            _cameraWidth = width;
            _cameraHeight = height;

            Vie = Matrix.Identity;//Matrix.CreateLookAt(new Vector3(0, 0, -100), Vector3.Zero, Vector3.Up);
            _projectionMatrix = Matrix.CreateOrthographic(_cameraWidth, _cameraHeight, -100000, 100000);
        }*/

        public void SetCamera(ICamera camera)
        {
            _cameraWidth = camera.Viewport.Width;
            _cameraHeight = camera.Viewport.Height;

            ViewMatrix = camera.ViewMatrix;//Matrix.Identity;
            ProjectionMatrix = camera.ProjectionMatrix; //Matrix.CreateOrthographic(_cameraWidth, _cameraHeight, -100000, 100000);
        }

        public void SetBackBufferDimensions(int width, int height) 
        {
            _gdm.PreferredBackBufferWidth = width;
            _gdm.PreferredBackBufferHeight = height;
            _gdm.ApplyChanges();
        }
    }
}
