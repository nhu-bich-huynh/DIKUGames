using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Input;
using BreakOut;
using DIKUArcade.Events;

namespace BreakOutStates;

/// <summary>
/// Represents the game state during the main menu screen.
/// </summary>
public class MainMenu : IGameState
{
    private static MainMenu? instance;
    private readonly Entity backGroundImage = new(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
        new Image(Path.Combine("Assets", "Images", "BreakoutTitleScreen.png")));
    private readonly Menu menu = new(new Text[] { new Text("New Game", new Vec2F(0.2f, 0.2f), new Vec2F(0.4f, 0.4f)),
                        new Text("Quit", new Vec2F(0.2f, 0.1f), new Vec2F(0.4f, 0.4f)) });
    public static MainMenu GetInstance()
    {
        if (MainMenu.instance == null)
        {
            MainMenu.instance = new MainMenu();
            MainMenu.instance.ResetState();
        }
        return MainMenu.instance;
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
    /// Scrolls the menu up and down, or selects the active menu button.
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
                                GameRunning.GetInstance().ResetState();
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
                                        EventType = GameEventType.WindowEvent,
                                        Message = "CLOSE_WINDOW"
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