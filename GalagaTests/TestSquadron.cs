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

public class TestSquadron {
    private ISquadron squad;
    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    [Test]
    public void TestLine()
    {
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
        squad = new Line();
        squad.CreateEnemies(enemyStridesBlue, enemyStridesRed);
        Assert.AreEqual(squad.MaxEnemies, 8);
    }

    [Test]
    public void TestLeftArrow()
    {
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
        squad = new LeftArrow();
        squad.CreateEnemies(enemyStridesBlue, enemyStridesRed);
        Assert.AreEqual(squad.MaxEnemies, 6);
    }

    [Test]
    public void TestRightArrow()
    {
        List<Image> enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("..", "Galaga", "Assets", "Images", "BlueMonster.png"));
        List<Image> enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("..", "Galaga", "Assets", "Images", "RedMonster.png"));
        squad = new RightArrow();
        squad.CreateEnemies(enemyStridesBlue, enemyStridesRed);
        Assert.AreEqual(squad.MaxEnemies, 6);
    }    
}