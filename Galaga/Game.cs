using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using System.Collections.Generic;
using Galaga.MovementStrategy;
using Galaga.Squadron;
using GalagaStates;
using Galaga.GalagaStates;

namespace Galaga
{
    public class Game : DIKUGame, IGameEventProcessor {
        private GameEventBus eventBus;
        private StateMachine stateMachine;
        public Game(WindowArgs windowArgs) : base(windowArgs) {
            eventBus = GalagaBus.GetBus();
            eventBus.InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, GameEventType.WindowEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent });
            window.SetKeyEventHandler(KeyHandler);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
            eventBus.Subscribe(GameEventType.PlayerEvent, this);
            eventBus.Subscribe(GameEventType.GameStateEvent, this);
            stateMachine = new StateMachine();
        }

        private void KeyHandler(KeyboardAction action, KeyboardKey key) {
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_RUNNING") {
                GameRunning.GetInstance().HandleKeyEvent(action, key);
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "MAIN_MENU") {
                if (action == KeyboardAction.KeyPress) MainMenu.GetInstance().HandleKeyEvent(action, key);
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_PAUSED") {
                if (action == KeyboardAction.KeyPress) GamePaused.GetInstance().HandleKeyEvent(action, key);
            }
        }

        public void ProcessEvent(GameEvent gameEvent) {
            switch (gameEvent.EventType) {
                case GameEventType.WindowEvent:
                    switch (gameEvent.Message) {
                        case "CLOSE_WINDOW":
                            window.CloseWindow();
                        break;
                    }
                break;
            }

        }

        public override void Render() {
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_PAUSED") {
                GamePaused.GetInstance().RenderState();
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "MAIN_MENU") {
                MainMenu.GetInstance().RenderState();
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_RUNNING") {
                GameRunning.GetInstance().RenderState();
            }
        }

        public override void Update() {
            eventBus.ProcessEventsSequentially();
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_PAUSED") {
                GamePaused.GetInstance().UpdateState();
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "MAIN_MENU") {
                MainMenu.GetInstance().UpdateState();
            }
            if (StateTransformer.TransformStateToString(stateMachine.ActiveState) == "GAME_RUNNING") {
                GameRunning.GetInstance().UpdateState();
            }
        }
    }
}
