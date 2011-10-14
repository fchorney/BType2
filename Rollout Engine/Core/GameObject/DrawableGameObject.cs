using Rollout.Drawing;

namespace Rollout.Core
{
    public class DrawableGameObject : GameObject, ITransformable
    {
        public float OffsetX { get; set; }

        public float OffsetY { get; set; }

        public float Scale { get; set; }

        public float Rotation { get; set; }


        public float X
        {
            get
            {
                if (Parent != null && Parent is DrawableGameObject)
                    return (Parent as DrawableGameObject).X + OffsetX;
                return OffsetX;
            }

            set { OffsetX = value; }
        }

        public float Y
        {
            get
            {
                if (Parent != null && Parent is DrawableGameObject)
                    return (Parent as DrawableGameObject).Y + OffsetY;
                return OffsetY;
            }

            set { OffsetY = value; }
        }
    }
}