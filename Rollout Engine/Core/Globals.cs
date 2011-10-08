using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Rollout.Core
{
    public class G
    {
        private static Game game;
        private static SpriteBatch spriteBatch;

        public static Game Game
        {
            get { return game; }
        }
        public static void SetGame(Game _game)
        {
            if (game == null)
            {
                game = _game;
            }
        }

        public static SpriteBatch SpriteBatch
        {
            get
            {
                if (spriteBatch == null)
                {
                    spriteBatch = new SpriteBatch(G.Game.GraphicsDevice);
                }
                return spriteBatch;
            }
        }

        public static ContentManager Content
        {
            get { return game.Content; }
        }
    }
}
