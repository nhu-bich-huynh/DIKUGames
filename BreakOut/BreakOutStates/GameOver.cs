using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Input;
using BreakOut;
using DIKUArcade.Events;
using DIKUArcade.Timers;

namespace BreakOutStates;

/// <summary>
/// Represents the game over state of the game.
/// </summary>
public class GameOver : IGameState, IGameEventProcessor
{
    private static GameOver? instance;
    protected Text? _display;
    protected Text? _text;
    protected int _points;
    protected Menu menu = new(new Text[] { new Text("Quit", new Vec2F(0.2f, 0.2f), new Vec2F(0.4f, 0.4f)),
                        new Text("Main Menu", new Vec2F(0.2f, 0.1f), new Vec2F(0.4f, 0.4f)) });
    private readonly Entity backGroundImage = new(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
        new Image(Path.Combine("Assets", "Images", "Overlay.png")));

    /// <summary>
    /// Returns the singleton instance of the GameOver state.
    /// </summary>
    public static GameOver GetInstance()
    {
        StaticTimer.PauseTimer();
        if (GameOver.instance == null)
        {
            GameOver.instance = new GameOver();
            GameOver.instance.ResetState();
        }
        return GameOver.instance;
    }

    /// <summary>
    /// Updates the state by updating the menu and displaying the score.
    /// </summary>
    public virtual void UpdateState()
    {
        menu.UpdateMenu();
        _display?.SetText("Score: " + _points.ToString());
    }

    /// <summary>
    /// Resets the state by subscribing to game events, resetting the menu, setting up the score display,
    /// and resetting the points.
    /// </summary>
    public void ResetState()
    {
        BreakOutBus.GetBus().Subscribe(GameEventType.StatusEvent, this);
        menu.ResetMenu();
        _display = new Text("Score: " + _points.ToString(), new Vec2F(0.2f, 0.3f), new Vec2F(0.4f, 0.4f));
        _display.SetColor(System.Drawing.Color.Blue);
        _points = 0;

    }

    /// <summary>
    /// Renders the state by rendering the background image, score display, text, and menu.
    /// </summary>
    public void RenderState()
    {
        backGroundImage.RenderEntity();
        _display?.RenderText();
        _text?.RenderText();
        menu.RenderMenu();
    }

    /// <summary>
    /// Handles keyboard events by processing key presses and registering game events accordingly.
    /// </summary>
    /// <param name="keyboardAction">The action of the keyboard event (e.g., key press).</param>
    /// <param name="keyboardKey">The key that triggered the keyboard event.</param>
    public void HandleKeyEvent(KeyboardAction keyboardAction, KeyboardKey keyboardKey)
    {
        if (keyboardAction == KeyboardAction.KeyPress)
        {
            switch (keyboardKey)
            {
                case (KeyboardKey.Up):
                    BreakOutBus.GetBus().RegisterEvent(new GameEvent
                    { EventType = GameEventType.GraphicsEvent, From = this, Message = "SWITCH_UP" });
                    break;
                case (KeyboardKey.Down):
                    BreakOutBus.GetBus().RegisterEvent(new GameEvent
                    { EventType = GameEventType.GraphicsEvent, From = this, Message = "SWITCH_DOWN" });
                    break;
                case (KeyboardKey.Enter):
                    switch (menu.ActiveButton())
                    {
                        case (0):
                            {
                                BreakOutBus.GetBus().RegisterEvent(
                                    new GameEvent
                                    {
                                        EventType = GameEventType.WindowEvent,
                                        Message = "CLOSE_WINDOW"
                                    }
                                );
                                break;
                            }
                        case (1):
                            {
                                BreakOutBus.GetBus().RegisterEvent(
                                    new GameEvent
                                    {
                                        EventType = GameEventType.GameStateEvent,
                                        Message = "MAIN_MENU"
                                    }
                                );
                                break;
                            }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Processes game events by handling specific event messages.
    /// </summary>
    /// <param name="gameEvent">The game event to process.</param>
    public void ProcessEvent(GameEvent gameEvent)
    {
        switch (gameEvent.Message)
        {
            case "Game Over Score":
                _text = new Text("You Lose!", new Vec2F(0.2f, 0.4f), new Vec2F(0.4f, 0.4f));
                _text.SetColor(System.Drawing.Color.Red);
                _points = gameEvent.IntArg1;
                break;
        }
    }
}