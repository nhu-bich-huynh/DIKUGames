using NUnit.Framework;
using BreakOut.EntityObjects;
using DIKUArcade.Math;

namespace BreakOutTests;
public class TestPlayerLives
{
    private PlayerLives? testSubject;
    private int InitialLives;

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        testSubject = new PlayerLives(new Vec2F(0.75f, 0.7f), new Vec2F(0.3f, 0.3f));

        InitialLives = testSubject.GetLives();
    }

    /// <summary>
    /// Verifies that the initial value of lives is positive. 
    /// </summary>
    [Test]
    public void TestInitialLives()
    {
        Assert.That(InitialLives, Is.GreaterThanOrEqualTo(0));
    }

    /// <summary>
    /// Verifies that the value of lives decrements when the player loses lives
    /// and it is never negative.
    /// </summary>
    [Test]
    public void TestLoseLives()
    {
        testSubject?.LoseLives();
        Assert.That(testSubject?.GetLives(), Is.LessThan(InitialLives));
        testSubject?.LoseLives();
        testSubject?.LoseLives();
        testSubject?.LoseLives();
        Assert.That(testSubject?.GetLives(), Is.GreaterThanOrEqualTo(0));
    }
}