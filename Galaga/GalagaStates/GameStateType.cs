using System;
using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Input;
using Galaga;
using DIKUArcade.Events;

namespace GalagaStates;

public enum GameStateType {
    GameRunning,
    GamePaused,
    MainMenu,
}
public static class StateTransformer {
    public static GameStateType TransformStringToState(string state) {
        switch (state) {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            default:
                throw new ArgumentException("Invalid argument: Not a GameState in String form!");
        }
    }
    public static string TransformStateToString(IGameState state) {
        switch (state) {
            case GameRunning gameRunning:
                return "GAME_RUNNING";
            case GamePaused gamePaused:
                return "GAME_PAUSED";
            case MainMenu mainMenu:
                return "MAIN_MENU";
            default:
                throw new ArgumentException("Invalid argument: Not a GameState!");
        }
    }
}