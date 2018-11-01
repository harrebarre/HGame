using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            const int prefWidth = 1600;
            const int prefHeight = 900;
            _graphics.PreferredBackBufferWidth = SysInfo.VWidth < prefWidth ? (int)Math.Round(SysInfo.VWidth * 0.99f) : prefWidth;
            _graphics.PreferredBackBufferHeight = SysInfo.VHeight < prefHeight ? (int)Math.Round(SysInfo.VHeight * 0.9f) : prefHeight;
            _graphics.IsFullScreen = false;
            //_graphics.PreferMultiSampling = true; // causes crash, bug with monogame 3.6, below is temporary fix
            _graphics.PreparingDeviceSettings += SetMultiSampling;
            _graphics.PreparingDeviceSettings += SetToPreserve;

            void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs e)
            {
                var pp = e.GraphicsDeviceInformation.PresentationParameters;
                pp.MultiSampleCount = 4;
            }

            void SetToPreserve(object sender, PreparingDeviceSettingsEventArgs eventargs)
            {
                eventargs.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            }

            Content.RootDirectory = "Content";

            IsMouseVisible = true;

            Window.IsBorderless = true;

            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

            void OnProcessExit(object sender, EventArgs e)
            {
                //
            }

            // Call update before every draw, instead of 60 times per second.
            IsFixedTimeStep = false;

            //this.IsFixedTimeStep = true;
            //this.TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 100);
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
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
