using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using StoryTime.Contexts;
using StoryTimeCore.Contexts.Interfaces;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Input.Time;
using StoryTimeFramework.Entities.Actors;
using StoryTimeFramework.WorldManagement;

namespace StoryTime
{
    public delegate void InitializeHandler();
    public delegate void LoadContentHandler();
    public delegate void UnLoadContentHandler();
    public delegate void UpdateHandler(WorldTime wt);
    public delegate void DrawHandler(IRenderer renderer, WorldTime wt);

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : XNAControl.XNAControlGame
    {
        private SpriteBatch spriteBatch;
        private Texture2D rectangle;
        private Static2DRenderableAsset _asset;
        private Actor _actor;

        private ITexture2D tex;

        private GameWorld _gameWorld;

        public event InitializeHandler OnInitialize;
        public event LoadContentHandler OnLoadContent;
        public event UnLoadContentHandler OnUnLoadContent;
        public event UpdateHandler OnUpdate;
        public event DrawHandler OnDraw;

        public GraphicsDeviceManager Graphics { get; private set; }
        public XNAGraphicsContext GraphicsContext { get; private set; }
        public GameWorld GameWorld 
        {
            get
            {
                return _gameWorld;
            }
        }

        public MyGame()
            :base("Content")
        {
        }

        public MyGame(IntPtr handle)
            : base(handle, "Content")
        {
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            if (OnInitialize != null)
                OnInitialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            if (OnLoadContent != null)
                OnLoadContent();

            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            GraphicsContext = new XNAGraphicsContext(this.GraphicsDeviceManager, this.Content);
            _gameWorld = new GameWorld(GraphicsContext);
            /*ITexture2D bitmap = GraphicsContext.LoadTexture2D("default");

            _asset = new Static2DRenderableAsset();
            _asset.Texture2D = bitmap;
            //GraphicsContext.SetSceneDimensions(1280, 720);
            _actor = new Actor()
            {
                RenderableAsset = _asset
            };

            Scene s = new Scene();
            s.AddWorldEntity(_actor);
            _gameWorld.AddScene(s);*/
            
            //rectangle = new Texture2D(GraphicsDevice, 2, 2);
            //rectangle.SetData(new[] { Color.White, Color.Red, Color.Green, Color.Blue });
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
            if (OnUnLoadContent != null)
                OnUnLoadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            var keyState = Keyboard.GetState();
            //Keys[] k = keyState.GetPressedKeys
            // TODO: Add your update logic here

            base.Update(gameTime);

            
            if (OnUpdate != null)
            {
                var wt = new WorldTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                GameWorld.Update(wt);
                OnUpdate(wt);
            }
        }

        //public void Draw(GraphicsDevice gd)
        //{
        //    if (rectangle == null)
        //    {
        //        rectangle = new Texture2D(gd, 2, 2);
        //        rectangle.SetData(new[] { Color.White, Color.Red, Color.Green, Color.Blue });
        //    }

        //    gd.Clear(Color.CornflowerBlue);
        //    var sb = new SpriteBatch(gd);
        //    sb.Begin();

        //    // Option One (if you have integer size and coordinates)
        //    sb.Draw(rectangle, new Rectangle(10, 20, 80, 30), null,
        //            Color.Chocolate, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);

        //    // Option Two (if you have floating-point coordinates)
        //    //spriteBatch.Draw(whiteRectangle, new Vector2(10f, 20f), null,
        //    //        Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f),
        //    //        SpriteEffects.None, 0f);

        //    sb.End();
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsContext.Clear(Color.CornflowerBlue);
            
            var render = GraphicsContext.GetRenderer();
            render.PreRender();
            
            if (OnDraw != null)
            {
                var wt = new WorldTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                OnDraw(render, wt);
            }
            //_asset.Render(render);
            GameWorld.RenderActiveScene();

            render.PostRender();
            base.Draw(gameTime);
        }
    }
}
