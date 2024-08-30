using NUnit.Framework;
using DIKUArcade.Events;
using DIKUArcade.State;
using BreakOutStates;
namespace BreakOutTests;
public class TestStateMachine
{
    private StateMachine testSubject = new StateMachine();

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    /// <summary>
    /// Unit test to ensure only one state can ever be active at a time
    /// </summary>
    [Test]
    public void StateSingularity()
    {
        Assert.That(testSubject.ActiveState, Is.EqualTo(MainMenu.GetInstance()));
        testSubject.ProcessEvent(new GameEvent { Message = "GAME_RUNNING" });
        Assert.That(testSubject.ActiveState, Is.EqualTo(GameRunning.GetInstance()));
        testSubject.ProcessEvent(new GameEvent { Message = "GAME_PAUSED" });
        Assert.That(testSubject.ActiveState, Is.EqualTo(GamePaused.GetInstance()));
        testSubject.ProcessEvent(new GameEvent { Message = "MAIN_MENU" });
        Assert.That(testSubject.ActiveState, Is.EqualTo(MainMenu.GetInstance()));
        testSubject.ProcessEvent(new GameEvent { Message = "GAME_OVER" });
        Assert.That(testSubject.ActiveState, Is.EqualTo(GameOver.GetInstance()));
        testSubject.ProcessEvent(new GameEvent { Message = "GAME_WINNING" });
        Assert.That(testSubject.ActiveState, Is.EqualTo(GameWinning.GetInstance()));

    }

    /// <summary>
    /// Unit test to ensure states implement the IGameState interface
    /// </summary>
    [Test]
    public void InterfaceImplementation()
    {
        Assert.That(MainMenu.GetInstance(), Is.AssignableTo<IGameState>());
        Assert.That(GameRunning.GetInstance(), Is.AssignableTo<IGameState>());
        Assert.That(GamePaused.GetInstance(), Is.AssignableTo<IGameState>());
        Assert.That(GameOver.GetInstance(), Is.AssignableTo<IGameState>());
        Assert.That(GameWinning.GetInstance(), Is.AssignableTo<IGameState>());
    }
}