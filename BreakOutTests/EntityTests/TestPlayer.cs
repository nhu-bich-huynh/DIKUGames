using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestPlayer
{
    private Player? testSubject;
    private float testSubjectInitialX;
    private float testSubjectInitialY;
    private DynamicShape? playerShape;
    private Image? playerImage;
    private Entity? testEntity;

    [SetUp]
    public void Setup()
    {
        DIKUArcade.GUI.Window.CreateOpenGLContext();

        playerShape = new DynamicShape(new Vec2F(0.4f, 0.1f), new Vec2F(0.2f, 0.03f));

        playerImage = new Image(Path.Combine("Assets", "Images", "player.png"));

        testSubject = new Player(playerShape, playerImage);

        testSubjectInitialX = testSubject.GetPosition().X;
        testSubjectInitialY = testSubject.GetPosition().Y;

        testEntity = new Entity(new DynamicShape(new Vec2F(0.4f, 0.1f), new Vec2F(0.2f, 0.03f)),
        new Image(Path.Combine("Assets", "Images", "player.png")));
    }


    /// <summary>
    /// Verifies that the player is within the screen bounds.
    /// </summary>
    [Test]
    public void TestPlayerIsInTheScreen()
    {
        Assert.That(testSubjectInitialX, Is.GreaterThan(0.0));
        Assert.That(testSubjectInitialX, Is.LessThan(1.0 - playerShape?.Extent.X));
        Assert.That(testSubjectInitialY, Is.GreaterThan(0.0));
        Assert.That(testSubjectInitialY, Is.LessThan(1.0));
    }

    /// <summary>
    /// Verifies that the player starts in the horizontal center of the screen.
    /// </summary>
    [Test]
    public void TestPlayerIsInTheCenter()
    {
        Assert.That(testSubjectInitialX, Is.GreaterThan(0.4));
        Assert.That(testSubjectInitialX, Is.LessThan(0.6));
    }


    /// <summary>
    /// Verifies that the player moves left when the 'MoveLeftPress' event is triggered.
    /// </summary>
    [Test]
    public void TestPlayerMoveLeftPress()
    {
        testSubject?.ProcessEvent(new GameEvent
        {
            EventType = GameEventType.PlayerEvent,
            From = this,
            Message = "MoveLeftPress"
        });
        testSubject?.Move();
        Assert.That(testSubject?.GetPosition().X, Is.LessThan(testSubjectInitialX));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));

    }

    /// <summary>
    /// Verifies that the player moves right when the 'MoveRightPress' event is triggered.
    /// </summary>
    [Test]
    public void TestPlayerMoveRightPress()
    {
        testSubject?.ProcessEvent(new GameEvent
        {
            EventType = GameEventType.PlayerEvent,
            From = this,
            Message = "MoveRightPress"
        });
        testSubject?.Move();
        Assert.That(testSubject?.GetPosition().X, Is.GreaterThan(testSubjectInitialX));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));

    }


    /// <summary>
    /// Verifies that the player should not be able to exit the left side of the screen and 
    // movement should be prohibited.
    /// </summary>
    [Test]
    public void TestPlayerMoveLeftExit()
    {
        int index = 0;
        while (index < 100)
        {
            testSubject?.ProcessEvent(new GameEvent
            {
                EventType = GameEventType.PlayerEvent,
                From = this,
                Message = "MoveLeftPress"
            });
            index++;
            testSubject?.Move();
        }
        Assert.That(testSubject?.GetPosition().X, Is.GreaterThan(0.01));
        Assert.That(testSubject?.GetPosition().X, Is.LessThan(1.0 - playerShape?.Extent.X));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));
    }

    /// <summary>
    /// Verifies that the player should not be able to exit the right side of the screen and 
    // movement should be prohibited.
    /// </summary>
    [Test]
    public void TestPlayerMoveRightExit()
    {
        int index = 0;
        while (index < 100)
        {
            testSubject?.ProcessEvent(new GameEvent
            {
                EventType = GameEventType.PlayerEvent,
                From = this,
                Message = "MoveRightPress"
            });
            index++;
            testSubject?.Move();
        }
        Assert.That(testSubject?.GetPosition().X, Is.GreaterThan(0.01));
        Assert.That(testSubject?.GetPosition().X, Is.LessThan(1.0 - playerShape?.Extent.X));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));
    }

    /// <summary>
    /// Verifies that the player must be a DIKUArcade Entity.
    /// </summary>
    [Test]
    public void TestPlayerIsEntity()
    {
        Assert.That(testSubject, Is.InstanceOf<Entity>());
    }

    /// <summary>
    /// Verifies that the player default to a rectangular shape.
    /// </summary>
    [Test]
    public void TestPlayerShape()
    {
        Assert.That(playerShape?.Extent.X, Is.GreaterThan(playerShape?.Extent.Y));
    }

    /// <summary>
    /// Verifies that the player exist in the bottom half of the screen.
    /// </summary>
    [Test]
    public void TestPlayerIsInTheBottomHalf()
    {
        Assert.That(testSubjectInitialY, Is.GreaterThan(0.0));
        Assert.That(testSubjectInitialY, Is.LessThan(0.15));
    }

    /// <summary>
    /// Verifies that the player stops moving left when the 'MoveLeftRelease' event is triggered.
    /// </summary>
    [Test]
    public void TestPlayerMoveLeftRelease()
    {
        testSubject?.ProcessEvent(new GameEvent
        {
            EventType = GameEventType.PlayerEvent,
            From = this,
            Message = "MoveLeftRelease"
        });
        testSubject?.Move();
        Assert.That(testSubject?.GetPosition().X, Is.EqualTo(testSubjectInitialX));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));

    }

    /// <summary>
    /// Verifies that the player stops moving right when the 'MoveRightRelease' event is triggered.
    /// </summary>
    [Test]
    public void TestPlayerMoveRightRelease()
    {
        testSubject?.ProcessEvent(new GameEvent
        {
            EventType = GameEventType.PlayerEvent,
            From = this,
            Message = "MoveRightRelease"
        });
        testSubject?.Move();
        Assert.That(testSubject?.GetPosition().X, Is.EqualTo(testSubjectInitialX));
        Assert.That(testSubject?.GetPosition().Y, Is.EqualTo(testSubjectInitialY));
    }
}