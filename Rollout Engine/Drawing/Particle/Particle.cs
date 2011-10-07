using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rollout.Drawing
{
    public interface IParticle : ITransformable
    {
        double Age { get; }
        double TimeToLive { get; }
        double ElapsedTime { get; }
        double[] Params { get; }
        bool Enabled { get; set; }
        TransformFunction Transform { get; }
        Color Color { get; set; }
        Sprite Sprite { get; set; }
    }

    public delegate void TransformFunction(IParticle p);

    public class Particle : IParticle
    {
        public double Age { get; private set; }
        public double TimeToLive { get; private set; }
        public double ElapsedTime { get; private set; }
        public double[] Params { get; private set; }
        public bool Enabled { get; set; }
        public TransformFunction Transform { get; private set; }
        public Color Color { get; set; }
        public Sprite Sprite { get; set; }

        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }

        // C# is dumb in that I can't modify struct values -_-'
        public float X
        {
            get { return Position.X; }
            set { Position = new Vector2(value,Position.Y); }
        }
        public float Y
        {
            get { return Position.Y; }
            set { Position = new Vector2(Position.X,value); }
        }

        public Particle(params double[] Parameters)
        {
            Init(Parameters);
        }

        public void Init(params double[] Parameters)
        {
            Age = 0;
            Params = Parameters;
            Position = new Vector2();
            Scale = 1f;
            Rotation = 0f;
            Transform = null;
        }

        public void Update(GameTime gameTime)
        {
            Age += gameTime.ElapsedGameTime.TotalSeconds;
            ElapsedTime = gameTime.ElapsedGameTime.TotalSeconds;

            Sprite.Update(gameTime);
            Transform(this);
        }

        public void Draw()
        {
            Sprite.Draw(X, Y, Color, Scale, Rotation);
        }
    }
}
