using System;
using System.Collections.Generic;
using System.Diagnostics;
using Rollout.Collision;
using Rollout.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Rollout.Drawing
{
    public interface ITransformable
    {
        float X { get; set; }
        float Y { get; set; }
        float Scale { get; set; }
        float Rotation { get; set; }
    }

    public class SpriteComponent : ITransformable
    {
        public Dictionary<string,Sprite> Sprites;
        private Sprite parentSprite;
        private Dictionary<int,List<Sprite>> order;
        
        public float X
        {
            get { return parentSprite.X; }
            set { parentSprite.X = value; }
        }

        public float Y
        {
            get { return parentSprite.Y; }
            set { parentSprite.Y = value; }
        }

        public float Scale
        {
            get { return parentSprite.Scale; }
            set
            {
                // This is tricky, since the subsprites offsets will have to
                // move with the parent sprite as it scales.
                parentSprite.Scale = value;
            }
        }

        public float Rotation
        {
            get { return parentSprite.Rotation; }
            set
            {
                // This is probably a trickier problem
                // Since we want it to rotate as a whole, and not have each individual sprite rotate independently
                parentSprite.Rotation = value;
            }
        }

        public Sprite this[string key]
        {
            get { return Sprites[key]; }
        }

        public SpriteComponent()
        {
            Sprites = new Dictionary<string, Sprite>();
            order = new Dictionary<int, List<Sprite>>();
        }

        public void AddSprite(string name, Sprite sprite,int drawOrder = 0, bool parent = false)
        {
            Sprites.Add(name, sprite);
            SetDrawOrder(name, drawOrder);

            if (parent)
                parentSprite = sprite;
        }

        public void SetDrawOrder(string name, int drawOrder)
        {
            Sprite sprite = Sprites[name];
            if(order.ContainsKey(sprite.drawOrder))
                order[sprite.drawOrder].Remove(sprite);

            sprite.drawOrder = drawOrder;
            
            if (!order.ContainsKey(drawOrder))
                order.Add(drawOrder,new List<Sprite>());
            order[drawOrder].Add(sprite);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite sprite in Sprites.Values)
            {
                sprite.Update(gameTime);
            }
        }

        public void Draw()
        {
            var list = order.Keys.ToList();
            list.Sort();
            foreach (int key in list)
            {
                foreach(Sprite sprite in order[key])
                {
                    if (sprite != parentSprite)
                        sprite.Draw(parentSprite);
                    else
                        sprite.Draw();
                }
            }
        }

    }

    public class Sprite : ITransformable
    {
        protected Vector2 position;
        private Animation animation;
        private Dictionary<string, Animation> animations;
        internal int drawOrder;

        public Animation Animation
        {
            get { return animation; }
        }

        public int H
        {
            get { return animation.SourceRectangle.Height; }
        }

        public int W
        {
            get { return animation.SourceRectangle.Width; }
        }

        public float X
        {
            get { return position.X; }
            set { position.X = value; }
        }

        public float Y
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        public float Scale { get; set; }
        public float Rotation { get; set; }
        public Color Color { get; set; }

        /// <summary>
        /// Create a new Sprite with a position.
        /// Optionally pass in a default animation name and animation.
        /// </summary>
        /// <param name="startPosition">Required: Start position of sprite</param>
        /// <param name="animationName">Optional: Default animation name (Required with animation parameter)</param>
        /// <param name="animation">Optional: Default animation (Required with animationName parameter)</param>
        public Sprite(Vector2 startPosition, Animation animation = null, string animationName = "main")
        {
            animations = new Dictionary<string, Animation>();
            position = startPosition;
            Color = Color.White;
            Scale = 1f;
            Rotation = 0f;
            drawOrder = 0;

            if (animation != null)
            {
                AddAnimation(animationName, animation);
            }
        }

        public void Update(GameTime gameTime)
        {
            animation.Update(gameTime);
        }

        public void Draw(float x, float y, Color color, float scale, float rotation)
        {
            Vector2 texCenter = new Vector2(W/2, H/2);
            G.SpriteBatch.Draw(animation.Texture, new Vector2(x + texCenter.X, y + texCenter.Y),animation.CurrentFrame.SourceRectangle,color, rotation, texCenter, scale, SpriteEffects.None, 0);
        }

        public virtual void Draw(ITransformable wrt = null)
        {
            // wrt = With Respect To
            // If an ITransformable object is passed in, this sprite is drawn wrt parent object
            if (wrt != null)
                Draw(wrt.X + X, wrt.Y + Y, Color, Scale, Rotation);
            else
                Draw(position.X, position.Y, Color, Scale, Rotation);
        }

        public void SetAnimation(string animationName)
        {
            animation = animations[animationName];
            animation.Reset();
        }

        public void Pause()
        {
            animation.Paused = true;
        }

        public void UnPause()
        {
            animation.Paused = false;
        }

        public void ReStart()
        {
            animation.Reset();
        }

        public bool isPaused()
        {
            return animation.Paused;
        }

        public void AddAnimation(string animationName, Animation animation)
        {
            animations.Add(animationName, animation);
            // If this is the first animation added
            // Set it to the current running animation
            if (animations.Count == 1)
                SetAnimation(animationName);
        }
    }
}
