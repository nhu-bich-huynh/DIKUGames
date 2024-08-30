using NUnit.Framework;
using DIKUArcade.Math;
using BreakOut.EntityObjects;
using BreakOut;

namespace BreakOutTests;
public class TestLevelTimeOut
{
    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();
    }

    /// <summary>
    /// Verifies that the value of time is set to -1 when level file does not contain time metadata
    /// </summary>
    [Test]
    public void ConvertTime_NoTimeMetaData()
    {

        Level level = LevelLoader.LoadLevel(Path.Combine("Assets", "Levels", "central-mass.txt"));
        LevelTimeOut time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));
        double number = time.GetTime();
        Assert.That(number, Is.EqualTo(-1));
    }

    /// <summary>
    /// Verifies that the value of time is converted correctly when level file contains time metadata
    /// </summary>
    [Test]
    public void ConvertTime_WithTimeMetaData()
    {
        Level level = LevelLoader.LoadLevel(Path.Combine("Assets", "Levels", "level3.txt"));
        LevelTimeOut time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));
        double number = time.GetTime();
        Assert.That(number, Is.EqualTo(180));
    }

    /// <summary>
    /// Verifies that when a level file contains time metadata, the value of time is reduced while
    /// the player is in the game running state
    /// </summary>
    [Test]
    public void CountDown_WithTimeMetaData()
    {
        Level level = LevelLoader.LoadLevel(Path.Combine("Assets", "Levels", "level1.txt"));
        LevelTimeOut time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));

        time.CountDown();
        Time.UpdateDeltaTime();
        time.CountDown();
        double number = time.GetTime();

        Assert.That(number, Is.LessThan(300));
    }

    /// <summary>
    /// Verifies that when a level file does not contain time metadata, the value of time is not processed.
    /// </summary>
    [Test]
    public void CountDown_NoTimeMetaData()
    {
        Level level = LevelLoader.LoadLevel(Path.Combine("Assets", "Levels", "columns.txt"));
        LevelTimeOut time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));

        time.CountDown();
        Time.UpdateDeltaTime();
        time.CountDown();

        double number = time.GetTime();

        Assert.That(number, Is.EqualTo(-1));
    }
}
