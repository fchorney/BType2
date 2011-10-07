using Microsoft.Xna.Framework;
using Rollout.Core;

namespace Rollout.Utility
{
    /// <summary>
    /// DrawManagerBegin is set to the very first drawable component.
    /// This will Begin the global spritebatch.
    /// </summary>
    public class DrawManagerBegin : DrawableGameComponent 
    {
        /// <summary>
        /// Create a new DrawManagerBegin with its draw order set to 0.
        /// </summary>
        public DrawManagerBegin() : base(G.Game)
        {
            DrawOrder = 0;
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.Begin();
            base.Draw(gameTime);
        }
    }

    /// <summary>
    /// DrawManagerEnd is set to the very last drawable component.
    /// This will End the global spritebatch.
    /// </summary>
    public class DrawManagerEnd : DrawableGameComponent
    {
        /// <summary>
        /// Create a new DrawManagerEnd with its draw order set to int.MaxValue.
        /// </summary>
        public DrawManagerEnd() : base(G.Game)
        {
            DrawOrder = int.MaxValue;
        }

        public override void Draw(GameTime gameTime)
        {
            G.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
