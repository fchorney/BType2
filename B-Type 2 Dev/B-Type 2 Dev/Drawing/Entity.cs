using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace B_Type_2_Dev.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }

    class Entity : ITransformable
    {
        private Frame _Frame;
        private Color _Color;
        private Vector2 _position;
        private float _scale;
        private float _rotation;

        public Frame Frame
        {
            get { return _Frame; }
        }

        public Animation Animation
        {
            get { return _Frame as Animation; }
        }

        public int Height
        {
            get { return _Frame.SourceRectangle.Height; }
        }

        public int Width
        {
            get { return _Frame.SourceRectangle.Width; }
        }

        public float X
        {
            get { return _position.X; }
            set { _position.X = value; }
        }

        public float Y
        {
            get { return _position.Y; }
            set { _position.Y = value; }
        }

        public float Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public Color Color
        {
            get { return _Color; }
            set { _Color = value; }
        }

        public Entity(Frame frame, Vector2 startPosition)
        {
            _Frame = frame;
            _position = startPosition;
            _Color = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            _Frame.Update(gameTime);
        }

        public void Draw()
        {
            //G.SpriteBatch.Draw(_Frame.Texture, _position, _Frame.SourceRectangle, _Color);
        }


        public void Draw(Color color)
        {
            //G.SpriteBatch.Draw(_Frame.Texture, _position, _Frame.SourceRectangle, color);
        }

        public void Draw(float x, float y, float scale, float rotation, Color color)
        {
            Vector2 texCenter = new Vector2(Width / 2, Height / 2);
            texCenter = new Vector2(0, 0);

            //G.SpriteBatch.Draw(_Frame.Texture, new Vector2(x + texCenter.X, y + texCenter.Y), _Frame.SourceRectangle, color, rotation, texCenter, scale, SpriteEffects.None, 0);
        }
    }
}
