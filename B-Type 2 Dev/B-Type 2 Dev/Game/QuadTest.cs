using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Box(int x, int y, int width, int height)
        {
            Shape = new Rectangle(x,y,width,height);
        }

        public void Collide (ICollidable obj)
        {
            
        }
    }

    public class QuadTest : DrawableGameComponent
    {
        private QuadTree quadTree;

        public QuadTest()
            : base(G.Game)
        {
            quadTree = new QuadTree(0,0,G.Game.GraphicsDevice.Viewport.Width,G.Game.GraphicsDevice.Viewport.Height);

            quadTree.Split();
            quadTree.Split();
            quadTree.Split();
            //quadTree.Split();
            //quadTree.Split();
            //quadTree.Split();
            //quadTree.Split();

            int rows = 33;
            int cols = 30;

            List<Box> boxes = new List<Box>();



            for(int i = 0; i < rows; i+=4)
            {
                for (int j = 0; j < cols; j++)
                {
                    Box b = new Box(i*5, j*5, 6, 6);
                    boxes.Add(b);
                    quadTree.Add(b);    
                }
                
            }

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


            TextWriter.Update("Collision", collisions.Count.ToString());
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();

            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
