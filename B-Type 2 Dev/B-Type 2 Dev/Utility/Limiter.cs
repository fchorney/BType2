using Microsoft.Xna.Framework;

namespace B_Type_2_Dev.Utility
{
    class Limiter
    {
        private double elapsedTime;

        public double Interval { get; set; }
        public bool Enabled { get; set; }
        
        public bool Ready
        {
            get 
            {
                bool result = elapsedTime >= Interval;
                if (result && Enabled)
                    elapsedTime = 0;
                return result;
            }
        }

        public Limiter(double interval)
        {
            Interval = interval;
            elapsedTime = double.MaxValue;
            Enabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
                elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Reset()
        {
            elapsedTime = 0;
        }

        public void Allow()
        {
            elapsedTime = Interval;
        }
    }
}
