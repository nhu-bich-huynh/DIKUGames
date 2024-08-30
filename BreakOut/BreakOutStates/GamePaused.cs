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
/// Represents the state of the game during a pause.
/// </summary>
public class GamePaused : IGameState
{
    private static GamePaused? instance;
    private readonly Menu menu = new(new Text[] { new Text("Continue", new Vec2F(0.2f, 0.2f), new Vec2F(0.4f, 0.4f)),
                    new Text("Main Menu", new Vec2F(0.2f, 0.1f), new Vec2F(0.4f, 0.4f)) });
    private readonly Entity backGroundImage = new(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
        new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
    public static GamePaused GetInstance()
    {
        StaticTimer.PauseTimer();

        if (GamePaused.instance == null)
        {
            GamePaused.instance = new GamePaused();
            GamePaused.instance.ResetState();
        }
        return GamePaused.instance;
    }

    public void UpdateState()
    {
        menu.UpdateMenu();
    }
    public void ResetState()
    {
        menu.ResetMenu();
    }
    public void RenderState()
    {
        backGroundImage.RenderEntity();
        menu.RenderMenu();
    }

    /// <summary>
    /// Scrolls the menu up and down, or the select the currently active menu button.
    /// </summary>
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
                                        EventType = GameEventType.GameStateEvent,
                                        Message = "GAME_RUNNING"
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
}