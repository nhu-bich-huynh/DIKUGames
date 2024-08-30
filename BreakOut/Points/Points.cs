using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace BreakOut
{
    /// <summary>
    /// Represents the points scored by the Player during the game.
    /// </summary>
    public class Points : IGameEventProcessor
    {
        private int points;
        private readonly Text display;

        /// <summary>
        /// Initializes an instance of the Points class.
        /// </summary>
        /// <param name="position">The position of the display of points on the screen.</param>
        /// <param name="extent">The extent of the display of points on the screen.</param>
        public Points(Vec2F position, Vec2F extent)
        {
            BreakOutBus.GetBus().Subscribe(GameEventType.StatusEvent, this);
            points = 0;
            display = new Text("Points: " + points.ToString(), position, extent);
            display.SetColor(System.Drawing.Color.White);
        }

        /// <summary>
        /// Increases the points by the given value.
        /// </summary>
        /// <param name="value">The value to increase the points by.</param>
        public void IncreasePoints(int value)
        {
            points += value;
            display.SetText("Points: " + points.ToString());
        }

        /// <summary>
        /// Renders the points display on the screen.
        /// </summary>
        public void RenderPoints()
        {
            display.RenderText();
        }

        /// <summary>
        /// Gets the current points.
        /// </summary>
        /// <returns>The current points.</returns>
        public int GetPoints()
        {
            return points;
        }

        /// <summary>
        /// Processes the game event, updates the points based on the event type and message.
        /// </summary>
        /// <param name="gameEvent">The game event to process.</param>
        public void ProcessEvent(GameEvent gameEvent)
        {
            if (gameEvent.EventType == GameEventType.StatusEvent)
            {
                switch (gameEvent.Message)
                {
                    case "BlockDestroyed":
                        IncreasePoints(gameEvent.IntArg1);
                        break;
                }
            }
        }

    }
}