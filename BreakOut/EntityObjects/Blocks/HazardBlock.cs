using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace BreakOut.EntityObjects;
/// <summary>
/// Represents the Hazard Block Type which spawns a random hazard upon death.
/// </summary>
public class HazardBlock : Block
{
    private HazardBlock(int value, int health, StationaryShape shape, IBaseImage image) :
        base(value, health, shape, image)
    { }
    public static HazardBlock HazardBlockFactory(StationaryShape shape, IBaseImage image)
    {
        return new HazardBlock(1, 1, shape, image);
    }

    /// <summary>
    /// Hits the block, similar to a regular block, but also has a 50% chance of spawning a hazard upon death.
    /// </summary>
    public override void Hit()
    {
        if (health == 1)
        {
            int random = BreakoutRandom.GetRand().Next(2);
            if (random == 1) EffectFactory.CreateRandomHazard(new DynamicShape
                (new Vec2F(Shape.Position.X, Shape.Position.Y),
                new Vec2F(0.05f, 0.05f), new Vec2F(0.0f, -0.005f)));
        }
        base.Hit();
    }
}