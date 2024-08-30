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
using Galaga;
using Galaga.GalagaStates;
using GalagaTests;
using GalagaStates;

namespace GalagaTests {
    [TestFixture]
    public class StateMachineTesting
    {
        private StateMachine stateMachine;
        private bool eventBusInitialized = false;

[SetUp]
        public void InitiateStateMachine()
        {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            stateMachine = new StateMachine();
            
            if (!eventBusInitialized) GalagaBus.GetBus().InitializeEventBus(new List<GameEventType> { GameEventType.InputEvent, GameEventType.WindowEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent });
            eventBusInitialized = true;
            GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, stateMachine);
            GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
            
        }
        [Test]
        public void TestInitialState()
        {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }
        [Test]
        public void TestEventGamePaused()
        {
            GalagaBus.GetBus().RegisterEvent(
            new GameEvent {
                EventType = GameEventType.GameStateEvent,
                Message = "GAME_PAUSED"
            });
            GalagaBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }
    [Test]
    public void TestEventGameRunning()
    {
        GalagaBus.GetBus().RegisterEvent(
        new GameEvent {
            EventType = GameEventType.GameStateEvent,
            Message = "GAME_RUNNING"
        });
        GalagaBus.GetBus().ProcessEventsSequentially();
        Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
    }
    }
}