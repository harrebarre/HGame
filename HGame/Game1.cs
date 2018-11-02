using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HGame
{
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            _graphics.IsFullScreen = false;
            if (_graphics.IsFullScreen)
            {
                _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            }
            else
            {
                const int prefWidth = 1600;
                const int prefHeight = 900;
                _graphics.PreferredBackBufferWidth = SysInfo.VWidth < prefWidth ? (int)Math.Round(SysInfo.VWidth * 0.99f) : prefWidth;
                _graphics.PreferredBackBufferHeight = SysInfo.VHeight < prefHeight ? (int)Math.Round(SysInfo.VHeight * 0.9f) : prefHeight;
            }

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

            Content.RootDirectory = "HContent";

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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
