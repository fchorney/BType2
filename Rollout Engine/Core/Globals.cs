using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Rollout.Core
{
    public class G
    {
        private static Game game;
        private static ManagedSpriteBatch spriteBatch;

        public static Game Game
        {
            get { return game; }
        }
        public static void SetGame(Game _game)
        {
            if (game != null) return;
            game = _game;
        }

        public static ManagedSpriteBatch SpriteBatch
        {
            get { return spriteBatch ?? (spriteBatch = new ManagedSpriteBatch()); }
        }

        public static ContentManager Content
        {
            get { return game.Content; }
        }
    }
}
