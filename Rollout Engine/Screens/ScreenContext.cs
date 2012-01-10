namespace Rollout.Screens
{
    public static class ScreenContext
    {
        internal static void SetContext(Screen screen)
        {
            _ContextKey = screen.GetType().ToString();
        }

        private static string _ContextKey;
        internal static string ContextKey
        {
            get { return _ContextKey ?? ""; }
        }
    }
}