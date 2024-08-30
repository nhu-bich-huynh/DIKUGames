using DIKUArcade.Events;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace BreakOut.EntityObjects
{
    using Modifiers;
    /// <summary>
    /// Represents the player lives during the game.
    /// </summary>
    public class PlayerLives : IFlagHandler
    {
        private int _lives;
        private readonly Text _display;

        /// <summary>
        /// Initializes an instance of the PlayerLives class.
        /// </summary>
        /// <param name="position">The position of the plaryer's lives display.</param>
        /// <param name="extent">The extent of the player's lives display.</param>
        public PlayerLives(Vec2F position, Vec2F extent)
        {
            _lives = 3;
            _display = new Text("Lives: " + _lives.ToString(), position, extent);
            _display.SetColor(System.Drawing.Color.White);

            Modifiers.Subscribe(this);
        }

        /// <summary>
        /// Decreases the number of lives by 1.
        /// </summary>
        public void LoseLives()
        {
            if (_lives > 0) _lives--;
            _display.SetText("Lives: " + _lives.ToString());
        }

        /// <summary>
        /// Renders the lives display on the screen.
        /// </summary>
        public void RenderLives()
        {
            _display.RenderText();
        }

        /// <summary>
        /// Gets the current number of lives.
        /// </summary>
        /// <returns>The current number of lives.</returns>
        public int GetLives()
        {
            return _lives;
        }


        /// <summary>
        /// Applies the specified game flag modifiers to the player lives.
        /// </summary>
        /// <param name="flag">The game flag modifiers to apply.</param>
        public void ApplyModifiers(GameFlag flag)
        {
            if (Modifiers.Added(flag).HasFlag(GameFlag.ExtraLife))
            {
                _lives++;
                _display.SetText("Lives: " + _lives.ToString());

                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.StatusEvent,
                    Message = "DisableExtraLife",
                });
            }

            if (Modifiers.Added(flag).HasFlag(GameFlag.LoseLife))
            {
                LoseLives();

                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.StatusEvent,
                    Message = "DisableLoseLife",
                });
            }
        }
    }
}
