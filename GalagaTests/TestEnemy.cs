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


namespace galagatests;

public class TestEnemy {
    private Enemy testSubject;
    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
        testSubject = new Enemy(new DynamicShape(new Vec2F(0.4f - 0.1f, 0.9f - 0.1f),
        new Vec2F(0.1f, 0.1f)),new ImageStride(80, enemyStridesBlue),
        new ImageStride(80, enemyStridesRed));
    }

    [Test]
    public void TestAliveState()
    {
        Assert.AreEqual(testSubject.State(), enemyState.Alive);
    }

    [Test]
    public void TestEnragedState() {
        testSubject.Hit();
        Assert.AreEqual(testSubject.State(), enemyState.Enraged);
    }
    [Test]
    public void TestDeadState() {
        testSubject.Hit();
        testSubject.Hit();
        Assert.AreEqual(testSubject.State(), enemyState.Dead);
    }
}