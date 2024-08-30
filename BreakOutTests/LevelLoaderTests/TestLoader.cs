using NUnit.Framework;
using BreakOut;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestLoader
{
    private string folder = Path.Combine("Assets", "Levels");

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    /// <summary>
    /// Unit test to ensure all levels are loaded successfully.
    /// </summary>
    [Test]
    public void TestAllLevels()
    {
        var level1 = LevelLoader.LoadLevel(Path.Combine(folder, "central-mass.txt"));
        var level2 = LevelLoader.LoadLevel(Path.Combine(folder, "columns.txt"));
        var level3 = LevelLoader.LoadLevel(Path.Combine(folder, "level1.txt"));
        var level4 = LevelLoader.LoadLevel(Path.Combine(folder, "level2.txt"));
        var level5 = LevelLoader.LoadLevel(Path.Combine(folder, "level3.txt"));
        var level6 = LevelLoader.LoadLevel(Path.Combine(folder, "wall.txt"));

        Assert.Multiple(() =>
        {
            Assert.That(level1, Is.Not.Null);
            Assert.That(level2, Is.Not.Null);
            Assert.That(level3, Is.Not.Null);
            Assert.That(level4, Is.Not.Null);
            Assert.That(level5, Is.Not.Null);
            Assert.That(level6, Is.Not.Null);
        });
    }

    /// <summary>
    /// Unit test to ensure it can handle differences in the metadata of loaded levels.
    /// </summary>
    [Test]
    public void TestDiffMeta()
    {
        var level1 = LevelLoader.LoadLevel(Path.Combine(folder, "columns.txt"));
        var level2 = LevelLoader.LoadLevel(Path.Combine(folder, "level1.txt"));

        Assert.Multiple(() =>
        {
            Assert.That(level1.Metas.TryGetValue("Name", out string? _), Is.True);
            Assert.That(level2.Metas.TryGetValue("Name", out string? _), Is.True);
            Assert.That(level2.Metas.TryGetValue("Time", out string? _), Is.True);
            Assert.That(level2.Metas.TryGetValue("Hardened", out string? _), Is.True);
            Assert.That(level2.Metas.TryGetValue("PowerUp", out string? _), Is.True);
        });
    }

    /// <summary>
    /// Unit test to verify that the data structures of a loaded level are as expected
    /// </summary>
    [Test]
    public void TestLevel()
    {
        var level = LevelLoader.LoadLevel(Path.Combine(folder, "level1.txt"));
        var blocks = new BlockFormation(level);

        Assert.Multiple(() =>
        {
            Assert.That(blocks.Container.CountEntities(), Is.GreaterThan(0));
            Assert.That(level.Metas["Name"], Is.EqualTo("LEVEL 1"));
            Assert.That(level.Legends["%"], Is.EqualTo("blue-block.png"));
        });
    }

    /// <summary>
    /// Unit test to ensure that it can handle invalid files without crashing the program.
    /// </summary>
    [Test]
    public void TestInvalid()
    {
        var level = LevelLoader.LoadLevel(Path.Combine(folder, "invalid.txt"));

        Assert.That(level, Is.Not.Null);
    }

    /// <summary>
    /// Unit test to ensure that it can handle empty files without crashing the program.
    /// </summary>
    [Test]
    public void TestEmpty()
    {
        var level = LevelLoader.LoadLevel(Path.Combine(folder, "empty.txt"));

        Assert.That(level, Is.Not.Null);
    }
}