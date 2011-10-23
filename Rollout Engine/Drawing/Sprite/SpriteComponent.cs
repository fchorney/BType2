using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Rollout.Drawing
{
    public class SpriteComponent : ITransformable
    {
        public Dictionary<string, Sprite> Sprites;
        private Sprite parentSprite;
        private Dictionary<int, List<Sprite>> order;

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

        public void AddSprite(string name, Sprite sprite, int drawOrder = 0, bool parent = false)
        {
            Sprites.Add(name, sprite);
            SetDrawOrder(name, drawOrder);

            if (parent)
                parentSprite = sprite;
        }

        public void SetDrawOrder(string name, int drawOrder)
        {
            Sprite sprite = Sprites[name];
            if (order.ContainsKey(sprite.drawOrder))
                order[sprite.drawOrder].Remove(sprite);

            sprite.drawOrder = drawOrder;

            if (!order.ContainsKey(drawOrder))
                order.Add(drawOrder, new List<Sprite>());
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
                foreach (Sprite sprite in order[key])
                {
                    if (sprite != parentSprite)
                        sprite.Draw(parentSprite);
                    else
                        sprite.Draw();
                }
            }
        }
    }
}
