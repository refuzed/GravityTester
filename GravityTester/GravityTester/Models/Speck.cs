﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GravityTester.Models
{
    public class Speck
    {
        private Vector2 _position;
        private Rectangle _shape;

        public float Mass { get; set; }
        public Vector2 Speed { get; set; }
        public Color Color { get; set; }
        public Texture2D Texture { get; set; }


        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                _shape.X = (int) value.X;
                _shape.Y = (int) value.Y;
            }
        }

        public Rectangle Shape
        {
            get { return _shape; }
            set
            {
                _shape = value;
                Position = new Vector2(value.X, value.Y);
            }
        }


        public Speck(Rectangle shape, Vector2 speed, Texture2D texture, Color color, float mass)
        {
            Shape = shape;
            Speed = speed;
            Color = color;
            Texture = texture;
            Mass = mass;
        }
    }
}
