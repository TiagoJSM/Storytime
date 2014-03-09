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
    public class GamePipeline : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D rectangle;
        XNAGraphicsContext _gContext;
        Static2DRenderableAsset _asset;

        ITexture2D tex;

        public event InitializeHandler OnInitialize;
        public event LoadContentHandler OnLoadContent;
        public event UnLoadContentHandler OnUnLoadContent;
        public event UpdateHandler OnUpdate;
        public event DrawHandler OnDraw;

        public GamePipeline()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            _gContext = new XNAGraphicsContext(this.graphics, this.Content);
            tex = _gContext.CreateTexture2D(
                new[] { Color.White, Color.Red, Color.Green, Color.Blue },
                2, 2,
                "DemoTex"
            );

            _asset = new Static2DRenderableAsset();
            _asset.SetBoundingBox(new StoryTimeCore.DataStructures.Rectanglef(10, 100.0f, 160));
            _asset.Texture2D = tex;
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

        public void Draw(GraphicsDevice gd)
        {
            if (rectangle == null)
            {
                rectangle = new Texture2D(gd, 2, 2);
                rectangle.SetData(new[] { Color.White, Color.Red, Color.Green, Color.Blue });
            }

            gd.Clear(Color.CornflowerBlue);
            var sb = new SpriteBatch(gd);
            sb.Begin();

            // Option One (if you have integer size and coordinates)
            sb.Draw(rectangle, new Rectangle(10, 20, 80, 30), null,
                    Color.Chocolate, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);

            // Option Two (if you have floating-point coordinates)
            //spriteBatch.Draw(whiteRectangle, new Vector2(10f, 20f), null,
            //        Color.Chocolate, 0f, Vector2.Zero, new Vector2(80f, 30f),
            //        SpriteEffects.None, 0f);

            sb.End();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _gContext.Clear(Color.CornflowerBlue);
            
            IRenderer render = _gContext.GetRenderer();
            render.PreRender();
            
            if (OnDraw != null)
            {
                WorldTime wt = new WorldTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                OnDraw(render, wt);
            }

            render.PostRender();
            base.Draw(gameTime);
        }
    }
}
