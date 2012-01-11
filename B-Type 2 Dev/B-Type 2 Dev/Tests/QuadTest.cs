using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Collision;
using Rollout.Collision.Shapes;
using Rollout.Core;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace Rollout.Game
{
    public class Box : ICollidable
    {
        public String Name { get; set; }

        public IShape Shape { get; private set; }

        public CollisionHandler OnCollision { get; set; }

        public bool Enabled { get; set; }

        public Box(int x, int y, int width, int height)
        {
            Shape = new Rectangle(x,y,width,height);
        }
    }

    public class QuadTest : DrawableGameComponent
    {
        private ICollisionEngine collisionEngine;

        public QuadTest()
            : base(G.Game)
        {
            collisionEngine = new QuadTreeCollisionEngine();

            int rows = 1;
            int cols = 2;

            List<Box> boxes = new List<Box>();



            for(int i = 0; i < rows; i+=4)
            {
                for (int j = 0; j < cols; j++)
                {
                    Box b = new Box(i*5, j*5, 6, 6) {Name = "Box" + (i*rows + j).ToString()};
                   
                    boxes.Add(b);
                    collisionEngine.Add(b);

                    b.OnCollision += HandleBoxCollision;
                }
                
            }
        }

        private void HandleBoxCollision(ICollidable source, ICollidable obj)
        {
            Box a = (Box) source;
            Box b = (Box) obj;
            Console.WriteLine(String.Format("{0} says ow! {1} collided with me! You son of a bitch!", a.Name, b.Name));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            collisionEngine.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();

            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
