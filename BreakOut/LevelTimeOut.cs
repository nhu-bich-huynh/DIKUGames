using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace BreakOut.EntityObjects
{
    /// <summary>
    /// Represents the level timeout in the game.
    /// </summary>
    public class LevelTimeOut
    {
        private readonly string? _time;
        private long number;
        private readonly Text _display;

        /// <summary>
        /// Initializes an instance of the LevelTimeOut class.
        /// </summary>
        /// <param name="level">The level object.</param>
        /// <param name="position">The position of the time display.</param>
        /// <param name="extent">The extent of the time display.</param>
        public LevelTimeOut(Level level, Vec2F position, Vec2F extent)
        {
            Dictionary<string, string> _meta = level.Metas;
            if (_meta.ContainsKey("Time")) _time = level.Metas["Time"];
            number = ConvertTime(_time);
            _display = new Text(_time, position, extent);
            _display.SetColor(System.Drawing.Color.White);
        }

        /// <summary>
        /// Renders the level time display on the screen.
        /// </summary>
        public void RenderLevelTimeOut()
        {
            if (number >= 0) _display.RenderText();
        }

        /// <summary>
        /// Converts the time from type string to milliseconds.
        /// </summary>
        /// <param name="time">The time string to convert.</param>
        /// <returns>The time in milliseconds.</returns>
        static private long ConvertTime(string? time)
        {
            long number = -1;

            if (time != null)
            {
                // convert to miliseconds
                number = long.Parse(time) * 1000;
            }

            return number;
        }

        /// <summary>
        /// Reduces the remaining time based on the elapsed delta time and update to the time display.
        /// </summary>
        public void CountDown()
        {
            if (number > 0)
            {
                number -= Time.DeltaTime();
                _display.SetText(Math.Floor(number / 1000.0).ToString());
            }
        }

        /// <summary>
        /// Gets the current remaining time in seconds.
        /// </summary>
        /// <returns>The current remaining time in seconds.</returns>
        public double GetTime()
        {
            double time = (Math.Floor(number / 1000.0));
            return time;
        }
    }
}