using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Input;
using BreakOutStates;

namespace BreakOut
{
    /// <summary>
    /// Represents the high-level updating and renderings logic of the game.
    /// </summary>
    public class Game : DIKUGame, IGameEventProcessor
    {
        private readonly StateMachine stateMachine;
        public Game(WindowArgs windowArgs) : base(windowArgs)
        {
            BreakOutBus.GetBus().InitializeEventBus(new List<GameEventType>
            { GameEventType.InputEvent, GameEventType.WindowEvent, GameEventType.PlayerEvent,
            GameEventType.GameStateEvent, GameEventType.GraphicsEvent, GameEventType.StatusEvent });
            window.SetKeyEventHandler(KeyHandler);
            BreakOutBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
            stateMachine = new StateMachine();
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key)
        {
            stateMachine.ActiveState.HandleKeyEvent(action, key);
        }

        public void ProcessEvent(GameEvent gameEvent)
        {
            switch (gameEvent.EventType)
            {
                case GameEventType.WindowEvent:
                    switch (gameEvent.Message)
                    {
                        case "CLOSE_WINDOW":
                            window.CloseWindow();
                            break;
                    }
                    break;
            }

        }

        public override void Render()
        {
            stateMachine.ActiveState.RenderState();
        }

        public override void Update()
        {
            BreakOutBus.GetBus().ProcessEventsSequentially();
            stateMachine.ActiveState.UpdateState();
        }
    }
}
