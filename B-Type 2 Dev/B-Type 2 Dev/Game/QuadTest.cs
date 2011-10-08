using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Rollout.Core;
using Rollout.Collision;
using Rollout.Drawing;
using Rollout.Utility;
using Rectangle = Rollout.Collision.Rectangle;

namespace Rollout.Game
{
    public class Box : ICollidable
    {
        public IShape Shape { get; private set; }
        public Sprite Sprite { get; set; }

        public Box(Sprite sprite)
        {
            Sprite = sprite;
            Shape = new Rectangle(sprite.X,sprite.Y,sprite.Width,sprite.Height);
        }

        public void Collide (ICollidable obj)
        {
            
        }
    }

    public class QuadTest : DrawableGameComponent
    {
        public Box Box_A { get; set; }
        public Box Box_B { get; set; }
        private QuadTree quadTree;

        public QuadTest()
            : base(G.Game)
        {
            Box_A = new Box(new Sprite(new Vector2(200, 200), "main", new Animation(@"Sprites/spaceship", 64, 64, 2, new double[] { .2, .2 })));
            Box_B = new Box(new Sprite(new Vector2(250, 200), "main", new Animation(@"Sprites/spaceship2", 64, 64, 2, new double[] { .2, .2 })));
            quadTree = new QuadTree(0,0,G.Game.GraphicsDevice.Viewport.Width,G.Game.GraphicsDevice.Viewport.Height);

            quadTree.Split();
            quadTree.Split();
            quadTree.Split();

            quadTree.Add(Box_A);
            quadTree.Add(Box_B);
        }

        public override void Initialize()
        {
            TextWriter.Add("Collision");
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            quadTree.CheckCollisions();
            PairList<ICollidable> collisions = quadTree.GetCollisions();

            Box_A.Sprite.Update(gameTime);
            Box_B.Sprite.Update(gameTime);

            TextWriter.Update("Collision", collisions.Count.ToString());
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();
            Box_A.Sprite.Draw();
            Box_B.Sprite.Draw();
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
