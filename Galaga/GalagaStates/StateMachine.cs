using DIKUArcade.Events;
using DIKUArcade.State;
using GalagaStates;
using System;

namespace Galaga.GalagaStates;
public class StateMachine : IGameEventProcessor {
    public IGameState ActiveState { get; private set; }
    public StateMachine() {
        GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        ActiveState = MainMenu.GetInstance();
        GameRunning.GetInstance();
        GamePaused.GetInstance();
    }
    private void SwitchState(GameStateType stateType) {
        switch (stateType) {
            case GameStateType.GameRunning:
                ActiveState = new GameRunning();
                break;
            case GameStateType.GamePaused:
                ActiveState = new GamePaused();
                break;
            case GameStateType.MainMenu:
                ActiveState = new MainMenu();
                break;
            default:
                throw new ArgumentException("Error: Not a game state type!");
        }
    }
    public void ProcessEvent(GameEvent gameEvent) {
        switch (gameEvent.Message) {
            case ("GAME_RUNNING"):
                SwitchState(GameStateType.GameRunning);
                break;
            case ("GAME_PAUSED"):
                GamePaused.GetInstance().ResetState();
                SwitchState(GameStateType.GamePaused);
                break;
            case ("MAIN_MENU"):
                MainMenu.GetInstance().ResetState();
                SwitchState(GameStateType.MainMenu);
                break;
            default: throw new ArgumentException("Error: StateMachine cannot process this event!");
        }
    }
}