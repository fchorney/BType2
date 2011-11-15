using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rollout.Core;
using Rollout.Utility;

namespace Rollout.Screens
{

    public class ScreenManager : DrawableGameComponent
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
        List<Screen> _ComponentsToUpdate;

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

        public ScreenManager()
            : base(G.Game)
        {
            Components = new List<Screen>();
            _ComponentsToUpdate = new List<Screen>();


            Transition = new Transition();
            Transition.OnTime = Time.s(1);
            Transition.OffTime = Time.s(1);
            Transition.Position = 1;
            ScreenState = ScreenState.TransitionOn;

            IsExiting = false;
            IsPopup = false;
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
            _ComponentsToUpdate.Clear();

            foreach (Screen screen in Components)
            {
                _ComponentsToUpdate.Add(screen);
            }

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

            // Loop as long as there are screens waiting to be updated.
            while (_ComponentsToUpdate.Count > 0)
            {
                // Pop the topmost screen off the waiting list.
                Screen screen = _ComponentsToUpdate[_ComponentsToUpdate.Count - 1];

                _ComponentsToUpdate.RemoveAt(_ComponentsToUpdate.Count - 1);

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

        }

     
        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            foreach (Screen screen in Components)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);

            }

        }

        #endregion

        #region Public Methods

        public void Add(Screen component)
        {

            if (component.ComponentManager == null)
            {
                //component.ComponentManager = this;
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
            _ComponentsToUpdate.Remove(component);

            component.ComponentManager = null;
        }

        #endregion

        #region DEBUG

        public void DrawDebugInfo(int x, int y)
        {
            int i = 12;
            foreach (Screen gc in Components)
            {
                i += i;
                G.SpriteBatch.DrawString(Game.Content.Load<SpriteFont>(@"SpriteFonts\Arial"), gc.ID + " " + gc.ScreenState.ToString(), new Vector2(x, y + i), Color.Red);

                gc.DrawDebugInfo(x + 40, y + 10);
            }

        }

        #endregion
    }
}