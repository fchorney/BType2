using Microsoft.Xna.Framework;
using Rollout.Scripting;

namespace Rollout.Core.GameObject
{
    public class DrawableGameObject : GameObject, IScriptable
    {
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }

        public virtual float X
        {
            get
            {
                if (Parent != null && Parent is DrawableGameObject)
                    return (Parent as DrawableGameObject).X + OffsetX;
                return OffsetX;
            }

            set { OffsetX = value; }
        }

        public virtual float Y
        {
            get
            {
                if (Parent != null && Parent is DrawableGameObject)
                    return (Parent as DrawableGameObject).Y + OffsetY;
                return OffsetY;
            }

            set { OffsetY = value; }
        }

        public Vector2 Position
        {
            get
            {
                return new Vector2(X,Y);
            }

            set 
            { 
                X = value.X;
                Y = value.Y;
            }
        }
    }
}