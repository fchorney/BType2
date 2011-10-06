using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rollout.Input
{
    /// <summary>
    /// The KeyboardInput class will take in a PlayerIndex and poll that players keyboard for input
    /// Extends the GameComponent class. (The input will poll on its own)
    /// </summary>
    public class KeyboardInput : IInputDevice
    {
        private KeyboardState prevState;
        private KeyboardState currState;
        private PlayerIndex playerIndex;

        /// <summary>
        /// Create a new KeyboardInput instance. Monitors the keyboard input for a specified player.
        /// </summary>
        /// <param name="pIndex">Specified Player Index</param>
        public KeyboardInput(PlayerIndex pIndex)
        {
            playerIndex = pIndex;
        }

        #region Initialize & Update
        public void Update(GameTime gameTime)
        {
            prevState = currState;
            currState = Keyboard.GetState(playerIndex);
        }
        #endregion

        #region Key Checks
        /// <summary>
        /// Check if a key has been pressed
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Returns true if key has been pressed</returns>
        public bool IsPressed(int key)
        {
            return prevState.IsKeyUp((Keys)key) && currState.IsKeyDown((Keys)key);
        }

        /// <summary>
        /// Check if a key has been released
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Returns true if key has been released</returns>
        public bool IsReleased(int key)
        {
            return prevState.IsKeyDown((Keys)key) && currState.IsKeyUp((Keys)key);
        }

        /// <summary>
        /// Check if a key is currently held
        /// </summary>
        /// <param name="key">Key to check</param>
        /// <returns>Returns true if key is held</returns>

        public bool IsHeld(int key)
        {
            return currState.IsKeyDown((Keys)key);
        }
        #endregion
    }
}
