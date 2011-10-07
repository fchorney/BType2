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
using Rollout.Drawing;
using Rollout.Input;
using Rollout.Core;
using Rollout.Utility;
using Rollout.Drawing.Examples;

namespace B_Type_2_Dev
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Sprite player;
        private PlayerInput input;
        private TextWriter textWriter;
        private ParticleEffect_A pEffect;

        private Texture2D whitePixel;
        private FPS fps;

        public Game1()
        {
            IsFixedTimeStep = false;

            graphics = new GraphicsDeviceManager(this)
                           {
                               IsFullScreen = false,
                               SynchronizeWithVerticalRetrace = false,
                               PreferredBackBufferHeight = 720,
                               PreferredBackBufferWidth = 1280
                           };
            graphics.ApplyChanges();


            Content.RootDirectory = "Content";
            G.Content = Content;
            G.Game = this;
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
            player = new Sprite(new Vector2(600,320f));
            player.AddAnimation("main", new Animation(@"Sprites/spaceship",64,64,2,new double[]{0.5f,1.0f},true,5));
            player.AddAnimation("alternate", new Animation(@"Sprites/spaceship2",64,64,2,new double[]{0.1f,0.2f}));
            //player.Animation.Loop = false;

            input = new PlayerInput(PlayerIndex.One);
            Components.Add(input);

            input.BindAction("Pause",Keys.P);
            input.BindAction("UnPause", Keys.U);
            input.BindAction("Switch-A",Keys.A);
            input.BindAction("Switch-S",Keys.S);
            input.BindAction("Restart",Keys.R);

            textWriter = new TextWriter(@"SpriteFonts/Debug");
            textWriter.Add("Paused");
            textWriter.Add("Particle Count");
            textWriter.Add("Particle Buffer Count");
            textWriter.Add("Enabled Particles");
            textWriter.Add("FPS");

            Sprite particleSprite = new Sprite(new Vector2(500f,300f));
            particleSprite.AddAnimation("main",new Animation(@"Sprites/Lensflare",256, 256, 1, new double[]{1}));
            pEffect = new ParticleEffect_A(particleSprite);

            fps = new FPS();

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
            G.SpriteBatch = spriteBatch;

            //To Draw a Primitive Rectangle or some shit
            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            whitePixel.Dispose();
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


            player.Update(gameTime);
            player.Rotation += (float)(20f * gameTime.ElapsedGameTime.TotalSeconds);
            player.Scale += (float)(.3f * gameTime.ElapsedGameTime.TotalSeconds);

            pEffect.Update(gameTime);
            fps.Update(gameTime);

            if (input.IsPressed("Pause"))
                player.Pause();

            if (input.IsPressed("UnPause"))
                player.UnPause();

            textWriter.Update("Paused", player.isPaused() ? "True" : "False");
            textWriter.Update("Particle Count", pEffect.Count.ToString());
            textWriter.Update("Particle Buffer Count",pEffect.BufferCount.ToString());
            textWriter.Update("Enabled Particles", (30000 - pEffect.BufferCount).ToString());
            textWriter.Update("FPS", fps.FrameRate.ToString());

            if (input.IsPressed("Switch-A"))
                player.SetAnimation("main");

            if (input.IsPressed("Switch-S"))
                player.SetAnimation("alternate");

            if (input.IsPressed("Restart"))
                player.ReStart();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            G.SpriteBatch.Begin();

            //G.SpriteBatch.Draw(whitePixel, G.Game.GraphicsDevice.Viewport.Bounds, Color.Blue);

            player.Draw();
            pEffect.Draw();
            fps.Draw();
            textWriter.Draw();

            G.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
