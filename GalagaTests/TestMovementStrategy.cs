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

public class TestMovementStrategy {
    private Enemy testSubject;
    private IMovementStrategy moveStrat;
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
    public void TestnoMove()
    {
        moveStrat = new NoMove();
        moveStrat.MoveEnemy(testSubject);
        Assert.AreEqual(testSubject.StartY, testSubject.Shape.Position.Y);
    }

    [Test]
    public void TestDown()
    {
        moveStrat = new Down();
        moveStrat.MoveEnemy(testSubject);
        Assert.AreEqual(testSubject.StartY - 0.0003f, testSubject.Shape.Position.Y);
    }

    [Test]
    public void TestZigZagDown()
    {
        moveStrat = new ZigZagDown();
        moveStrat.MoveEnemy(testSubject);
        Assert.AreEqual(testSubject.StartY - 0.0003f, testSubject.Shape.Position.Y);
        Assert.AreEqual(0.302093714f, testSubject.Shape.Position.X);
    }
}