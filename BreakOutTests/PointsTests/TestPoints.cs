using NUnit.Framework;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut;
using BreakOut.EntityObjects;

namespace BreakOutTests;
public class TestPoints
{
    private int InitialPoints;
    private Block? block;
    private blockState InitialState;
    private Points? playerReward;
    private int InitialHealth;
    private int value;

    [SetUp]
    public void Setup()
    {
        BreakOutBus.GetBus().InitializeEventBus(new List<GameEventType> 
            { GameEventType.InputEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent, 
            GameEventType.GraphicsEvent, GameEventType.StatusEvent });

        DIKUArcade.GUI.Window.CreateOpenGLContext();
        
        block = new Block(new StationaryShape(1.0f, 1.0f, 1.0f / 12.0f, 1.0f / 50.0f),
        new Image(Path.Combine("Assets", "Images", "blue-block.png")));
        InitialState = block.State();
        playerReward = new Points(new Vec2F(0.1f, 0.6f), new Vec2F(0.4f, 0.4f));
        InitialPoints = playerReward.GetPoints();

        InitialHealth = block.GetHealth();
        InitialState = block.State();
        value = block.GetValue();
    }

    [Test]
    public void PointsTests()
    {
        // Verifies that Points must at all times be a positive number.

        Assert.That(InitialPoints, Is.GreaterThanOrEqualTo(0));

        // Verifies that Points should be awarded when the player successfully destroys a block.

        block?.Hit();
        block?.Hit();
        BreakOutBus.GetBus().ProcessEventsSequentially();
        Assert.That(playerReward?.GetPoints(), Is.GreaterThan(InitialPoints));
        Assert.That(playerReward?.GetPoints(), Is.GreaterThanOrEqualTo(0));

        // Verifes that Points should be awarded based on the block’s value.

        block?.Hit();
        block?.Hit();
        BreakOutBus.GetBus().ProcessEventsSequentially();
        Assert.That(playerReward?.GetPoints(), Is.EqualTo(value));
        Assert.That(playerReward?.GetPoints(), Is.GreaterThanOrEqualTo(0));
    }

}