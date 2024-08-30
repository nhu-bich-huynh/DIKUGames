using DIKUArcade.State;

namespace BreakOutStates;

/// <summary>
/// This class is responsible for translating between strings and game states.
/// </summary>
public static class StateTransformer
{
    /// <summary>
    /// Returns a game state corresponding to a given string
    /// </summary>
    public static GameStateType TransformStringToState(string state)
    {
        switch (state)
        {
            case "GAME_RUNNING":
                return GameStateType.GameRunning;
            case "GAME_PAUSED":
                return GameStateType.GamePaused;
            case "MAIN_MENU":
                return GameStateType.MainMenu;
            case "GAME_OVER":
                return GameStateType.GameOver;
            case "GAME_WINNING":
                return GameStateType.GameWinning;
            default:
                throw new ArgumentException("Invalid argument: Not a GameState in String form!");
        }
    }

    /// <summary>
    /// Returns a string corresponding to a given Game State.
    /// </summary>
    public static string TransformStateToString(IGameState state)
    {
        switch (state)
        {
            case GameRunning:
                return "GAME_RUNNING";
            case GamePaused:
                return "GAME_PAUSED";
            case MainMenu:
                return "MAIN_MENU";
            case GameOver:
                return "GAME_OVER";
            case GameWinning:
                return "GAME_WINNING";
            default:
                throw new ArgumentException("Invalid argument: Not a GameState!");
        }
    }
}