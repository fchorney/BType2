using System;
using System.Collections.Generic;
using Rollout.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rollout.Input
{
    public interface IInputDevice
    {
        bool IsPressed(int code);
        bool IsHeld(int code);
        bool IsReleased(int code);
    }

    /// <summary>
    /// The PlayerInput provides user-defined action triggers binded to device input
    /// </summary>
    public class PlayerInput : GameComponent
    {

        private delegate bool Function();
        private Dictionary<string, Function> actionsPressed;
        private Dictionary<string, Function> actionsReleased;
        private Dictionary<string, Function> actionsHeld;
        private PlayerIndex playerIndex;

        private KeyboardInput keyboard;
        private ControllerInput controller;

        /// <summary>
        /// Create a new PlayerInput instance. Monitors the input for a specified player.
        /// </summary>
        /// <param name="pIndex">Specified Player Index</param>
        public PlayerInput(PlayerIndex pIndex)
            : base(G.Game)
        {
            playerIndex = pIndex;
            actionsPressed = new Dictionary<string, Function>();
            actionsReleased = new Dictionary<string, Function>();
            actionsHeld = new Dictionary<string, Function>();
            keyboard = new KeyboardInput(pIndex);
            controller = new ControllerInput(pIndex);

            G.Game.Components.Add(this);
        }

        #region Initialize & Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            keyboard.Update(gameTime);
            controller.Update(gameTime);
        }

        #endregion

        public void BindAction(string actionCode, Keys key)
        {
            string keyCode = key.ToString();
            BindAction(actionCode, "Keys", keyCode);
        }

        public void BindAction(string actionCode, Buttons button)
        {
            string keyCode = button.ToString();
            BindAction(actionCode, "Buttons", keyCode);
        }

        public void BindAction(string actionCode, string enumName, string keyCode)
        {
            string controlType = enumName;
            string controlKey = keyCode;
            int codeValue;
            IInputDevice device;
            Type enumType;

            switch (controlType)
            {
                case "Keys":
                    device = keyboard;
                    enumType = typeof(Keys);
                    break;
                case "Buttons":
                    device = controller;
                    enumType = typeof(Buttons);
                    break;
                default:
                    throw new Exception("Invalid input type");
            }

            codeValue = (int)Enum.Parse(Type.GetType(enumType.AssemblyQualifiedName), controlKey, true);

            Function isPressedFunction = () => device.IsPressed(codeValue);
            Function isReleasedFunction = () => device.IsReleased(codeValue);
            Function isHeldFunction = () => device.IsHeld(codeValue);

            if (actionsPressed.ContainsKey(actionCode))
                actionsPressed[actionCode] += isPressedFunction;
            else
                actionsPressed.Add(actionCode, isPressedFunction);

            if (actionsReleased.ContainsKey(actionCode))
                actionsReleased[actionCode] += isReleasedFunction;
            else
                actionsReleased.Add(actionCode, isReleasedFunction);

            if (actionsHeld.ContainsKey(actionCode))
                actionsHeld[actionCode] += isHeldFunction;
            else
                actionsHeld.Add(actionCode, isHeldFunction);

        }

        public void BindActionToMultiple(string actionCode, string enumName, string kCodes)
        {
            string controlType = enumName;
            string[] keyCodes = kCodes.Split(',');
            int[] controlKeys = new int[keyCodes.Length];

            IInputDevice device;
            Type enumType;

            switch (controlType)
            {
                case "Keys":
                    device = keyboard;
                    enumType = typeof(Keys);
                    break;
                case "Buttons":
                    device = controller;
                    enumType = typeof(Buttons);
                    break;
                default:
                    throw new Exception("Invalid input type");
            }

            for (int i = 0; i < keyCodes.Length; i++)
                controlKeys[i] = ((int)Enum.Parse(Type.GetType(enumType.AssemblyQualifiedName), keyCodes[i], true));

            Function isPressedFunction = delegate()
            {
                bool hasPressed = false;
                for (int i = 0; i < controlKeys.Length; i++)
                {
                    if (!device.IsHeld(controlKeys[i]))
                        return false;
                    if (device.IsPressed(controlKeys[i]))
                        hasPressed = true;
                }
                return hasPressed;
            };

            Function isReleasedFunction = () =>
                                              {
                                                  for (int i = 0; i < controlKeys.Length; i++)
                                                      if (!device.IsReleased(controlKeys[i])) return false;
                                                  return true;
                                              };

            Function isHeldFunction = () =>
                                          {
                                              for (int i = 0; i < controlKeys.Length; i++)
                                                  if (!device.IsHeld(controlKeys[i])) return false;
                                              return true;
                                          };

            if (actionsPressed.ContainsKey(actionCode))
                actionsPressed[actionCode] += isPressedFunction;
            else
                actionsPressed.Add(actionCode, isPressedFunction);

            if (actionsReleased.ContainsKey(actionCode))
                actionsReleased[actionCode] += isReleasedFunction;
            else
                actionsReleased.Add(actionCode, isReleasedFunction);

            if (actionsHeld.ContainsKey(actionCode))
                actionsHeld[actionCode] += isHeldFunction;
            else
                actionsHeld.Add(actionCode, isHeldFunction);
        }

        public bool IsPressed(string actionCode)
        {
            return GetActionValue(actionCode, actionsPressed);
        }

        public bool IsReleased(string actionCode)
        {
            return GetActionValue(actionCode, actionsReleased);
        }

        public bool IsHeld(string actionCode)
        {
            return GetActionValue(actionCode, actionsHeld);
        }

        private bool GetActionValue(string actionCode, Dictionary<string, Function> actionList)
        {
            if (actionList.ContainsKey(actionCode))
            {
                Delegate[] dels = actionList[actionCode].GetInvocationList();

                for (int i = 0; i < dels.Length; i++)
                {
                    Function checkValue = dels[i] as Function;
                    if (checkValue()) return true;
                }
            } //else do we throw an error?
            return false;
        }
    }
}
