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

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D canvas;
        Rectangle tracedSize;
        KeyboardState keyboard;
        KeyboardState old_keyboard;
        Random rnd;
        RenderTarget2D buffer;
        SpriteFont font;
        Double roughness;
        int seed;
        int size;
        int dunnowhattocallthis;

        Planet planet;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;
            //this.graphics.IsFullScreen = true;
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
            tracedSize = GraphicsDevice.PresentationParameters.Bounds;
            canvas = new Texture2D(GraphicsDevice, tracedSize.Width, tracedSize.Height, false, SurfaceFormat.Color);
            rnd = new Random();
            buffer = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);

            old_keyboard = new KeyboardState();

            roughness = 1.5;
            seed = 0;
            size = 1;
            dunnowhattocallthis = 100;
            planet = new Planet(1536*size, seed, roughness);

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
            font = Content.Load<SpriteFont>("Courier New");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            keyboard = Keyboard.GetState(PlayerIndex.One);

            if (keyboard.IsKeyDown(Keys.Escape))
                this.Exit();

            if (keyboard.IsKeyDown(Keys.A)  && old_keyboard.IsKeyUp(Keys.A))  seed -= 1;
            if (keyboard.IsKeyDown(Keys.D) && old_keyboard.IsKeyUp(Keys.D)) seed += 1;
            if (keyboard.IsKeyDown(Keys.W)     && old_keyboard.IsKeyUp(Keys.W) && size < 6)  size += 1;
            if (keyboard.IsKeyDown(Keys.S)     && old_keyboard.IsKeyUp(Keys.S) && size > 1)  size -= 1;
            //if (keyboard.IsKeyDown(Keys.A)) seed += 1;
            //if (keyboard.IsKeyDown(Keys.D)) seed -= 1;
            if (keyboard.IsKeyDown(Keys.Q)) roughness -= 0.1;
            if (keyboard.IsKeyDown(Keys.E)) roughness += 0.1;
            if (keyboard.IsKeyDown(Keys.Down)) dunnowhattocallthis -= 1;
            if (keyboard.IsKeyDown(Keys.Up)) dunnowhattocallthis += 1;
            if (keyboard.IsKeyDown(Keys.Enter))
            {
                planet = new Planet(1536 * size, seed, roughness);
                planet.map.generateWorld(dunnowhattocallthis);
            }

            old_keyboard = keyboard;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            GraphicsDevice.SetRenderTarget(buffer);
            GraphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);

            //draw some stuff.

            if (keyboard.IsKeyDown(Keys.Space))
                planet.map.drawWorld(spriteBatch, tracedSize);

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            //GraphicsDevice.Clear(Color.Blue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(buffer, Vector2.Zero, Color.White);

            planet.drawCircle(spriteBatch, tracedSize);
            planet.map.drawLine(spriteBatch, tracedSize);
            spriteBatch.DrawString(font, "A/D Seed: " + seed.ToString(), new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "Q/E Roughness: " + roughness.ToString(), new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(font, "W/S Size: " + size.ToString(), new Vector2(0, 40), Color.White);
            spriteBatch.DrawString(font, "Up/Down Caves: " + dunnowhattocallthis.ToString(), new Vector2(0, 60), Color.White);
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
