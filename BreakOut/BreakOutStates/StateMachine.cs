using DIKUArcade.Events;
using DIKUArcade.State;
using BreakOut;
using System.Reflection;

namespace BreakOutStates;
/// <summary>
/// The Statemachine is responsible for switching between game states and keeping track of
/// which one is currently active.
/// </summary>
public class StateMachine : IGameEventProcessor
{
    public IGameState ActiveState { get; private set; }
    public StateMachine()
    {
        BreakOutBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
        ActiveState = MainMenu.GetInstance();
    }

    /// <summary>
    /// Switches the currently active state to a new one. This method was inspired by a code example from Adaptive Code by Hall (2017)
    /// from the course curriculum.
    /// </summary>
    private void SwitchState(GameStateType stateType)
    {
        string stateTypeName = stateType.ToString();
        Type? stateClass = Type.GetType("BreakOutStates." + stateTypeName);
        MethodInfo? getInstanceMethod = stateClass?.GetMethod("GetInstance", BindingFlags.Public | BindingFlags.Static);
        object? gameState = getInstanceMethod?.Invoke(null, null);

        if (stateType == GameStateType.MainMenu)
        {
            MainMenu.GetInstance().ResetState();
        }

        if (stateType == GameStateType.GamePaused)

        {
            GamePaused.GetInstance().ResetState();

        }

        if (gameState != null)
        {
            ActiveState = (IGameState)gameState;
        }
    }

    /// <summary>
    /// Switches to a given game state, contained in the message of a registered event.
    /// </summary>
    public void ProcessEvent(GameEvent gameEvent)
    {
        SwitchState(StateTransformer.TransformStringToState(gameEvent.Message));
    }
}