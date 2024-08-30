using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestBlock
{
    private Block? testSubject;
    private int InitialHealth;

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        testSubject = new Block(new StationaryShape(1.0f, 1.0f, 1.0f / 12.0f, 1.0f / 50.0f),
        new Image(Path.Combine("Assets", "Images", "blue-block.png")));

        InitialHealth = testSubject.GetHealth();
    }


    /// <summary>
    /// Verifies that every Block is a DIKUArcade Entity
    /// </summary>
    [Test]
    public void TestEntity()
    {
        Assert.That(testSubject, Is.InstanceOf<Entity>());
    }

    /// <summary>
    /// Verifies that every Block has a Value and Health property
    /// </summary>
    [Test]
    public void TestHealthAndValueProperty()
    {
        Assert.Multiple(() =>
        {
            Assert.That(testSubject?.GetHealth(), Is.Not.Null);
            Assert.That(testSubject?.GetValue(), Is.Not.Null);
        });
    }

    /// <summary>
    /// Verifies that when a block is "hit", its health decrements
    /// </summary>
    [Test]
    public void TestHit()
    {
        testSubject?.Hit();
        Assert.That(testSubject?.GetHealth(), Is.LessThan(InitialHealth));
    }

    /// <summary>
    /// Verifies that when a block's health reaches zero, it is deleted.
    /// </summary>
    [Test]
    public void TestDeath()
    {
        testSubject?.Hit();
        testSubject?.Hit();

        Assert.That(testSubject, Is.Not.Null);
        Assert.That(testSubject!.IsDeleted());
    }
}