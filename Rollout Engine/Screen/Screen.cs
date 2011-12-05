using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;
using Rollout.Input;
using Rollout.Scripting;
using Rollout.Utility;

namespace Rollout.Screens
{
    /// <summary>
    /// Enum describes the screen transition state.
    /// </summary>
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden
    }

    /// <summary>
    /// A screen is a single layer that has update and draw logic, and which
    /// can be combined with other layers to build up a complex menu system.
    /// For instance the main menu, the options menu, the "are you sure you
    /// want to quit" message box, and the main game itself are all implemented
    /// as screens.
    /// </summary>
    public abstract class Screen : DrawableGameObject
    {
        #region Properties

        public Transition Transition { get; set; }
        public string ID { get; set; }

        /// <summary>
        /// Collection of components this component manages
        /// </summary>
        //public List<Screen> Components
        //{
        //    get { return _Components; }
        //}

        public List<Screen> Components { get; internal set; }
        private List<Screen> componentsToUpdate;

        /// <summary>
        /// Gets the scripting engine that belongs to the screen
        /// </summary>
        public ScriptingEngine scriptingEngine { get; private set; }

        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public Screen ComponentManager { get; internal set; }

        /// <summary>
        /// Normally when one screen is brought up over the top of another,
        /// the first screen will transition off to make room for the new
        /// one. This property indicates whether the screen is only a small
        /// popup, in which case screens underneath it do not need to bother
        /// transitioning off.
        /// </summary>
        public bool IsPopup { get; set; }
        public bool IsPersistant { get; set; }

        /// <summary>
        /// Gets the current screen transition state.
        /// </summary>
        public ScreenState ScreenState { get; protected set; }

        /// <summary>
        /// There are two possible reasons why a screen might be transitioning
        /// off. It could be temporarily going away to make room for another
        /// screen that is on top of it, or it could be going away for good.
        /// This property indicates whether the screen is exiting for real:
        /// if set, the screen will automatically remove itself as soon as the
        /// transition finishes.
        /// </summary>
        public bool IsExiting { get; protected internal set; }

        /// <summary>
        /// Checks whether this screen is active and can respond to user input.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return !HasFocus &&
                       (ScreenState == ScreenState.TransitionOn ||
                        ScreenState == ScreenState.Active);
            }
        }

        bool HasFocus;

        #endregion

        #region Initialization

        public Screen()
        {
            Components = new List<Screen>();
            componentsToUpdate = new List<Screen>();

            Transition = new Transition
                             {
                                 OnTime = Time.s(1), 
                                 OffTime = Time.s(1), 
                                 Position = 1
                             };
            ScreenState = ScreenState.TransitionOn;

            IsExiting = false;
            IsPopup = false;

            Screen = this;

            scriptingEngine = new ScriptingEngine();
        }

        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public new virtual void LoadContent() { }

        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public new virtual void UnloadContent() { }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        /// 

        public override void Update(GameTime gameTime)
        {
            // Read the keyboard and gamepad.
            //input.Update();

            // Make a copy of the master screen list, to avoid confusion if
            // the process of updating one screen adds or removes others.
            componentsToUpdate.Clear();

            foreach (Screen screen in Components)
            {
                componentsToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = false;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (componentsToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                Screen screen = componentsToUpdate[componentsToUpdate.Count - 1];

                componentsToUpdate.RemoveAt(componentsToUpdate.Count - 1);

                // Update the screen.
                screen.Update(gameTime);
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    // If this is the first active screen we came across,
                    // give it a chance to handle input.
                    if (!otherScreenHasFocus)
                    {
                        //screen.HandleInput(input);
                        otherScreenHasFocus = true;
                    }

                    // If this is an active non-popup, inform any subsequent
                    // screens that they are covered by it.
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

            base.Update(gameTime);
        }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            HasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                ScreenState = ScreenState.TransitionOff;

                if (!Transition.Update(gameTime, Transition.OffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    if (ComponentManager != null)
                        ComponentManager.Remove(this);
                }
            }
            else if (coveredByOtherScreen && !IsPersistant)
            {
                // If the screen is covered by another, it should transition off.
                if (Transition.Update(gameTime, Transition.OffTime, 1))
                {
                    // Still busy transitioning.
                    ScreenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    ScreenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (Transition.Update(gameTime, Transition.OnTime, -1))
                {
                    // Still busy transitioning.
                    ScreenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    ScreenState = ScreenState.Active;
                }
            }
            scriptingEngine.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput(PlayerInput input) { }

        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (var screen in Components.Where(screen => screen.ScreenState != ScreenState.Hidden))
            {
                screen.Draw(gameTime);
            }
            base.Draw(gameTime);
        }

        #endregion

        #region Public Methods

        public void AddScreen(Screen component)
        {
            if (component.ComponentManager == null)
            {
                component.ComponentManager = this;
                component.IsExiting = false;
                component.Initialize();

                Components.Add(component);
            }
            else
            {
                //exception here?
                //throw new Exception("Component is already being managed by another component");
            }
        }

        public void Remove(Screen component)
        {
            Components.Remove(component);
            componentsToUpdate.Remove(component);
            component.ComponentManager = null;
        }

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            if (Transition.OffTime == TimeSpan.Zero)
            {
                // If the screen has a zero transition time, remove it immediately.
                ComponentManager.Remove(this);
            }
            else
            {
                // Otherwise flag that it should transition off and then exit.
                IsExiting = true;
            }
        }

        public static void FadeBackBufferToBlack(int alpha)
        {
            Viewport viewport = G.Game.GraphicsDevice.Viewport;
            Texture2D blankTexture = G.Content.Load<Texture2D>("blank");

            G.SpriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             new Color(255, 255, 255, (byte)alpha));
        }
        #endregion

        #region DEBUG

        public void DrawDebugInfo(int x, int y)
        {
            int i = 12;
            foreach (Screen gc in Components)
            {
                i += i;
                G.SpriteBatch.DrawString(G.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), gc.ID + " " + gc.ScreenState.ToString(), new Vector2(x, y + i), Color.Red);

                gc.DrawDebugInfo(x + 40, y + 10);
            }

        }

        #endregion
    }
}