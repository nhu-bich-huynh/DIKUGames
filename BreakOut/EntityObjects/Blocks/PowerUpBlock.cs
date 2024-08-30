using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;


namespace BreakOut.EntityObjects;
/// <summary>
/// Represents the PowerUp Block Type, which spawns a random PowerUp upon death.
/// </summary>
public class PowerUpBlock : Block
{
    private PowerUpBlock(int value, int health, StationaryShape shape, IBaseImage image) :
        base(value, health, shape, image)
    {
    }
    public static PowerUpBlock PowerUpBlockFactory(StationaryShape shape, IBaseImage image)
    {
        return new PowerUpBlock(1, 1, shape, image);
    }

    /// <summary>
    /// Hits the block, similar to the regular block type, but also spawns a power-up upon death.
    /// </summary>
    public override void Hit()
    {
        if (health == 1)
            EffectFactory.CreateRandomPowerUp(new DynamicShape(new Vec2F(Shape.Position.X, Shape.Position.Y),
                new Vec2F(0.05f, 0.05f), new Vec2F(0.0f, -0.005f)));
        base.Hit();
    }
}