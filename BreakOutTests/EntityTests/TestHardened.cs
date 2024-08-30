using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestHardened
{

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    // The Hardened block has twice the amount of health and value of a normal block.
    [Test]
    public void TestHealthAndValue()
    {
        Block block = new Block(new StationaryShape(1.0f, 1.0f, 1.0f / 12.0f, 1.0f / 50.0f),
            new Image(Path.Combine("Assets", "Images", "blue-block.png")));

        Hardened hardened = Hardened.HardenedFactory(new StationaryShape
            (1.0f, 1.0f - 1.0f, 1.0f / 12.0f, 1.0f / 40.0f),
            new Image(Path.Combine("Assets", "Images", "blue-block.png")),
            new Image(Path.Combine("Assets", "Images", "blue-block-damaged.png")));

        Assert.That(hardened.GetHealth() == block.GetHealth() * 2);
        Assert.That(hardened.GetValue() == block.GetValue() * 2);
    }

    // When a hardened block goes below half its total health, its image must be changed to a damaged image
    [Test]
    public void TestDamagedImage()
    {
        Image normalImage = new Image(Path.Combine("Assets", "Images", "blue-block.png"));
        Image damagedImage = new Image(Path.Combine("Assets", "Images", "blue-block-damaged.png"));

        Hardened hardened = Hardened.HardenedFactory(new StationaryShape
                (1.0f, 1.0f - 1.0f, 1.0f / 12.0f, 1.0f / 40.0f),
                normalImage, damagedImage);

        Assert.That(hardened.Image, Is.EqualTo(normalImage));

        hardened.Hit();
        hardened.Hit();
        hardened.Hit();

        Assert.That(hardened.Image, Is.EqualTo(damagedImage));
    }
}