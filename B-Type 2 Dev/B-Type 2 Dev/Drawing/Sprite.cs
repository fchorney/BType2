using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using B_Type_2_Dev.Globals;

namespace B_Type_2_Dev.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }

    class Sprite : ITransformable
    {
        private Animation _Frame;
        private Vector2 _position;

        public Animation Animation
        {
            get { return _Frame as Animation; }
        }

        public int Height
        {
            get { return _Frame.CurrentFrame.SourceRectangle.Height; }
        }

        public int Width
        {
            get { return _Frame.CurrentFrame.SourceRectangle.Width; }
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

        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }

        public Sprite(Animation frame, Vector2 startPosition)
        {
            _Frame = frame;
            _position = startPosition;
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;
        }

        public void Update(GameTime gameTime)
        {
            _Frame.Update(gameTime);
        }

        public void Draw()
        {
            //G.SpriteBatch.Draw(_Frame.Texture, _position, _Frame.CurrentFrame.SourceRectangle, _Color);
            Draw(_position.X, _position.Y, Scale, Rotation, Color);
        }


        public void Draw(Color color)
        {
            G.SpriteBatch.Draw(_Frame.Texture, _position, _Frame.CurrentFrame.SourceRectangle, color);
        }

        public void Draw(float x, float y, float scale, float rotation, Color color)
        {
            Vector2 texCenter = new Vector2(Width / 2, Height / 2);
           // texCenter = new Vector2(0, 0);

            G.SpriteBatch.Draw(_Frame.Texture, new Vector2(x + texCenter.X, y + texCenter.Y), _Frame.CurrentFrame.SourceRectangle, color, rotation, texCenter, scale, SpriteEffects.None,0);
        }
    }
}
