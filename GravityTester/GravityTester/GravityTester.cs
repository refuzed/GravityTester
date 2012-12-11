using System;
using System.Collections.Generic;
using GravityTester.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityTester
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GravityTester : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        private int _maxX;
        private int _maxY;
        private readonly Vector2 _maxSpeed = new Vector2(5,5);

        private List<Speck> _specks;

        private const double _gravitationalConstant = .25; 

        public GravityTester()
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
            IsMouseVisible = true;
            _maxX = graphics.GraphicsDevice.Viewport.Width;
            _maxY = graphics.GraphicsDevice.Viewport.Height;
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

            var dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            dummyTexture.SetData(new[] { Color.White });

            Random seed = new Random();

            _specks = new List<Speck>();

            for (int i = 0; i < 3; i++)
            {
                var massMod = seed.Next(1, 5);
                _specks.Add(new Speck(new Rectangle(seed.Next(0, _maxX), seed.Next(0, _maxY), 5 * massMod, 5 * massMod),
                                      new Vector2(seed.Next(-2, 2), seed.Next(-2, 2)), dummyTexture, Color.White, massMod));
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            CheckGravity();
            CheckWallBounces();
            UpdatePositions();

            base.Update(gameTime);
        }

        private void CheckGravity()
        {
            foreach (var s in _specks)
            {
                Vector2 result = new Vector2();
                foreach(var s2 in _specks)
                {
                    if (object.ReferenceEquals(s, s2)) { continue; }
                    Vector2 diff = s.Position - s2.Position;
                    float magnitide = (float) Math.Sqrt((diff.X*diff.X) + (diff.Y*diff.Y));
                    float factor = ((float)_gravitationalConstant*((s.Mass*s2.Mass)/(magnitide*magnitide)))/s.Mass;
                    diff *= factor;
                    result += diff;
                }
                s.Speed += result;
                s.Speed = new Vector2(Math.Min(s.Speed.X, _maxSpeed.X), Math.Min(s.Speed.Y, _maxSpeed.Y));
            }
        }

        private void CheckWallBounces()
        {
            foreach (var s in _specks)
            {
                if(s.Position.Y < 0 )
                {
                    s.Speed = new Vector2(s.Speed.X, -Math.Abs(s.Speed.Y));
                }
                else if (s.Position.Y > _maxY - s.Shape.Height)
                {
                    s.Speed = new Vector2(s.Speed.X, Math.Abs(s.Speed.Y));
                }
                else if(s.Position.X < 0)
                {
                    s.Speed = new Vector2(-Math.Abs(s.Speed.X), s.Speed.Y);
                }
                else if(s.Position.X > _maxX - s.Shape.Width)
                {
                    s.Speed = new Vector2(Math.Abs(s.Speed.X), s.Speed.Y);
                }
            }
        }

        private void UpdatePositions()
        {
            foreach (var s in _specks)
            {
                s.Position -= s.Speed;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var s in _specks)
            {
                spriteBatch.Draw(s.Texture, s.Shape, s.Color);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
