using System;
using System.Collections.Generic;
using Rollout.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rollout.Utility
{
    public class TextWriter
    {
        private SpriteFont font;
        private int x, y;
        private Dictionary<string, TextObject> text;

        public TextWriter(string assetName)
        {
            font = G.Content.Load<SpriteFont>(assetName);
            text = new Dictionary<string, TextObject>();
            x = y = 20;
        }

        public void Add(string label)
        {
            text.Add(label, new TextObject(new Vector2(x,y)));
            y += 25;
        }

        public void Update(string label, string data)
        {
            text[label].Data = data;
        }

        public void Draw()
        {
            foreach (KeyValuePair<string, TextObject> pair in text)
            {
                G.SpriteBatch.DrawString(font, pair.Key + ": " + pair.Value.Data, pair.Value.Position, Color.White);
            }
        }
    }

    public class TextObject
    {
        public string Data { get; set; }
        public Vector2 Position { get; private set; }

        public TextObject(Vector2 position = default(Vector2))
        {
            Console.WriteLine("Vector 2: X:" + position.X + " Y:" + position.Y);
            Data = "";
            Position = position;
        }
    }
}
