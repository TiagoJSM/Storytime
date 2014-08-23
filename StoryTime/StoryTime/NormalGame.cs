using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StoryTimeFramework.Resources.Graphic;
using StoryTimeCore.Contexts.Interfaces;
using StoryTime.Contexts;
using StoryTimeCore.Entities.Actors;
using StoryTimeFramework.WorldManagement;
using StoryTimeCore.WorldManagement;
using Microsoft.Xna.Framework.Input;
using StoryTimeCore.Input.Time;

namespace StoryTime
{
    public class NormalGame : Game
    {
        SpriteBatch spriteBatch;
        Texture2D rectangle;
        Static2DRenderableAsset _asset;
        Actor _actor;

        ITexture2D tex;

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
                if (_gameWorld == null)
                    _gameWorld = new GameWorld();
                return _gameWorld;
            }
        }

        public NormalGame()
        {
            Graphics = new GraphicsDeviceManager(this);
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
            Content.RootDirectory = "Content";

            GraphicsContext = new XNAGraphicsContext(this.Graphics, this.Content);
            ITexture2D bitmap = GraphicsContext.LoadTexture2D("Bitmap1");

            _asset = new Static2DRenderableAsset();
            //_asset.SetBoundingBox(new StoryTimeCore.DataStructures.Rectanglef(0, 0.0f, 720, 1280));
            _asset.Texture2D = bitmap;
            GraphicsContext.SetSceneDimensions(1280, 720);
            _actor = new Actor()
            {
                RenderableAsset = _asset
            };

            Scene s = new Scene();
            s.AddActor(_actor);
            GameWorld.AddScene(s);
            GameWorld.GraphicsContext = GraphicsContext;
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
            KeyboardState keyState = Keyboard.GetState();
            //Keys[] k = keyState.GetPressedKeys
            // TODO: Add your update logic here

            base.Update(gameTime);

            
            if (OnUpdate != null)
            {
                WorldTime wt = new WorldTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                OnUpdate(wt);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsContext.Clear(Color.CornflowerBlue);
            
            IRenderer render = GraphicsContext.GetRenderer();
            render.PreRender();
            
            if (OnDraw != null)
            {
                WorldTime wt = new WorldTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                OnDraw(render, wt);
            }
            //_asset.Render(render);
            GameWorld.RenderActiveScene();

            render.PostRender();
            base.Draw(gameTime);
        }
    }
}
