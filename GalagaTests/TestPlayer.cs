namespace galagatests;
using NUnit.Framework;
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

public class TestPlayer {
    private Player testSubject;
    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
        testSubject = new Player(
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("..", "Galaga", "Assets", "Images", "Player.png")));
    }

    [Test]
    public void TestMoveLeft()
    {
        testSubject.ProcessEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveLeftPress" });
        testSubject.Move();
        Assert.AreEqual(0.439999998f, testSubject.GetPosition().X);
    }

    [Test]
    public void TestMoveRight()
    {
        testSubject.ProcessEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightPress" });
        testSubject.Move();
        Assert.AreEqual(0.459999979f, testSubject.GetPosition().X);
    }
}