using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestUnbreakable
{

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    /// <summary>
    /// Verifies that the Unbreakable block cannot be broken
    /// </summary>
    [Test]
    public void TestUnbreakability()
    {
        var unbreakable = Unbreakable.UnbreakFactory(new StationaryShape
            (1.0f, 1.0f - 1.0f, 1.0f / 12.0f, 1.0f / 40.0f),
            new Image(Path.Combine("Assets", "Images", "blue-block.png")));
        var block = new Block((new StationaryShape(1.0f, 1.0f - 1.0f, 1.0f / 12.0f, 1.0f / 40.0f)),
            new Image(Path.Combine("Assets", "Images", "blue-block.png")));

        for (int x = 0; x < 100; x++) unbreakable.Hit();
        for (int x = 0; x < 100; x++) block.Hit();

        Assert.That(unbreakable.IsDeleted() == false);
        Assert.That(block.IsDeleted());
    }
}