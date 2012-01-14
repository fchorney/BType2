using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Rollout.Drawing.Sprites;
using Rectangle = Rollout.Collision.Shapes.Rectangle;

namespace B_Type_2_Dev
{
    public class Player : Sprite
    {
        public Dictionary<string, Gun> Guns { get; set; }

        public Player()
        {
            Position = new Vector2(500,600);
            AddAnimation("main", Animation.Load("player"));
            Name = "player";
            Shape = new Rectangle(0, 0, 64, 64);

            Guns = new Dictionary<string, Gun>();
            Guns.Add("left", new Gun("left", new Vector2(-21, 20), new Vector2(6, -18)));
            Guns.Add("right", new Gun("right", new Vector2(57, 20), new Vector2(6, -18)));

            foreach (var gun in Guns.Values)
            {
                Add(gun.Sprite);
            }
        }

    }
}