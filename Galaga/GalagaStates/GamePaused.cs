using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Input;
using Galaga;
using DIKUArcade.Events;

namespace GalagaStates;

public class GamePaused : IGameState {
    private static GamePaused instance = null;
    private Entity backGroundImage = new Entity(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)), 
        new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
    private Text[] menuButtons = {new Text("Continue", new Vec2F(0.2f,0.2f) , new Vec2F(0.4f,0.4f)), new Text("Main Menu" , new Vec2F(0.2f,0.1f) , new Vec2F(0.4f,0.4f))};
    private int activeMenuButton = 0;
    public static GamePaused GetInstance() {
        if (GamePaused.instance == null) {
            GamePaused.instance = new GamePaused();
            GamePaused.instance.ResetState();
        }
        return GamePaused.instance;
    }
    public void UpdateState() {
        menuButtons[activeMenuButton].SetColor(System.Drawing.Color.Green);
        if (activeMenuButton == 1) menuButtons[0].SetColor(System.Drawing.Color.White);
        if (activeMenuButton == 0) menuButtons[1].SetColor(System.Drawing.Color.White);

    }
    public void ResetState() {
        activeMenuButton = 0;
        menuButtons[0].SetColor(System.Drawing.Color.Green);
        menuButtons[1].SetColor(System.Drawing.Color.White);
    }
    public void RenderState() {
        backGroundImage.RenderEntity();
        foreach (Text text in menuButtons) {
            text.RenderText();
        }
    }

    public void HandleKeyEvent(KeyboardAction keyboardAction, KeyboardKey keyboardKey) {
        switch (keyboardAction) {
            case (KeyboardAction.KeyPress):
                switch (keyboardKey) {
                    case (KeyboardKey.Up):
                        if (activeMenuButton == 1) activeMenuButton = 0;
                        break;
                    case (KeyboardKey.Down):
                        if (activeMenuButton == 0) activeMenuButton = 1;
                        break;
                    case (KeyboardKey.Enter):
                        if (activeMenuButton == 0) {
                            GalagaBus.GetBus().RegisterEvent(
                                new GameEvent{
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "GAME_RUNNING"
                                }
                            );
                        }
                        if (activeMenuButton == 1) {
                            GalagaBus.GetBus().RegisterEvent(
                                new GameEvent{
                                    EventType = GameEventType.GameStateEvent,
                                    Message = "MAIN_MENU"
                                }
                            );
                        }
                        break;
                }
            break;
        }
    }
}