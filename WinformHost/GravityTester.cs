using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GravityTester.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WinformHost
{
    public class GravityTester : GraphicsDeviceControl
    {
        ContentManager content;
        SpriteBatch spriteBatch;

        private int _maxX;
        private int _maxY;
        private readonly Vector2 _maxSpeed = new Vector2(5,5);
        private Texture2D _dummyTexture;

        public int Speed { get; set; }
        public double GravitationalNotsoConstant { get; set; }
        
        private List<Speck> _specks;

        protected override void Initialize()
        {
            content = new ContentManager(Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //font = content.Load<SpriteFont>("hudFont");

            _maxX = GraphicsDevice.Viewport.Width;
            _maxY = GraphicsDevice.Viewport.Height;
            Speed = 60;
            GravitationalNotsoConstant = .25;

            _dummyTexture = new Texture2D(GraphicsDevice, 1, 1);
            _dummyTexture.SetData(new[] { Color.White });

            Random seed = new Random();

            _specks = new List<Speck>();

            for (int i = 0; i < 3; i++)
            {
                var massMod = seed.Next(1, 5);
                _specks.Add(new Speck(new Rectangle(seed.Next(0, _maxX), seed.Next(0, _maxY), 5 * massMod, 5 * massMod),
                                      new Vector2(seed.Next(-2, 2), seed.Next(-2, 2)), _dummyTexture, Color.White, massMod));
            }

            Application.Idle += delegate { Invalidate(); };
        }

        /// <summary>
        /// Disposes the control, unloading the ContentManager.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Unload();
            }

            base.Dispose(disposing);
        }

        private void DoMath()
        {
            CheckGravity();
            CheckWallBounces();
            UpdatePositions();
        }

        private void CheckGravity()
        {
            foreach (var s in _specks)
            {
                Vector2 result = new Vector2();
                foreach(var s2 in _specks)
                {
                    if (object.ReferenceEquals(s, s2)) { continue; }
                    result += GravityMath(s, s2);
                }
                s.Speed += result;
                s.Speed = new Vector2(Math.Min(s.Speed.X, _maxSpeed.X), Math.Min(s.Speed.Y, _maxSpeed.Y));
            }
        }

        private Vector2 GravityMath(Speck s, Speck s2)
        {
            Vector2 diff = new Vector2(s.Shape.Center.X, s.Shape.Center.Y) - new Vector2(s2.Shape.Center.X, s2.Shape.Center.Y);
            float magnitide = (float) Math.Sqrt((diff.X*diff.X) + (diff.Y*diff.Y));
            float factor = ((float) GravitationalNotsoConstant*((s.Mass*s2.Mass)/(magnitide*magnitide)))/s.Mass;
            diff *= factor;
            return diff;
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

        public void MakeBaby()
        {
            var seed = new Random();
            var massMod = seed.Next(1, 5);
            _specks.Add(new Speck(new Rectangle(seed.Next(0, _maxX), seed.Next(0, _maxY), 5 * massMod, 5 * massMod),
                                  new Vector2(seed.Next(-2, 2), seed.Next(-2, 2)), _dummyTexture, Color.White, massMod));
        }

        public void Nuke()
        {
            _specks = new List<Speck>();
        }

        protected override void Draw()
        {
            if(Speed != 0)
            {
                DoMath();
                System.Threading.Thread.Sleep(1000 / Speed);
            }

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var s in _specks)
            {
                spriteBatch.Draw(s.Texture, s.Shape, s.Color);
            }

            spriteBatch.End();
        }
    }
}
