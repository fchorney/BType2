using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace B_Type_2_Dev.Input
{
    /// <summary>
    /// The ControllerInput class will take in a PlayerIndex and poll that players controller for input
    /// Extends the GameComponent class. (The input will poll on its own)
    /// </summary>
    public class ControllerInput : IInputDevice
    {
        private GamePadState prevState;
        private GamePadState currState;
        private PlayerIndex playerIndex;

        /// <summary>
        /// Create a new ControllerInput instance. Monitors the controller input for the specified player
        /// </summary>
        /// <param name="pIndex">Specified Player Index</param>
        public ControllerInput(PlayerIndex pIndex)
        {
            playerIndex = pIndex;
        }

        #region Initialize & Update

        public void Update(GameTime gameTime)
        {
            prevState = currState;
            currState = GamePad.GetState(playerIndex);
        }

        #endregion

        #region Button Checks
        /// <summary>
        /// Check if a button has been pressed
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>Returns true if button has been pressed</returns>
        public bool IsPressed(int button)
        {
            return prevState.IsButtonUp((Buttons)button) && currState.IsButtonDown((Buttons)button);
        }

        /// <summary>
        /// Check if button has been released
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>Returns true if button has been pressed</returns>
        public bool IsReleased(int button)
        {
            return prevState.IsButtonDown((Buttons)button) && currState.IsButtonUp((Buttons)button);
        }

        /// <summary>
        /// Check if button is currently held
        /// </summary>
        /// <param name="button">Button to check</param>
        /// <returns>Returns true if button is held</returns>
        public bool IsHeld(int button)
        {
            return currState.IsButtonDown((Buttons)button);
        }
        #endregion
    }
}
