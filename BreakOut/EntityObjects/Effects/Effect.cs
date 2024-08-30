using DIKUArcade.Entities;
using DIKUArcade.Graphics;

namespace BreakOut.EntityObjects;

/// <summary>
/// An abstract class providing basic logic for special effects to inherit from.
/// </summary>
public abstract class Effect : Entity
{
    public Effect(DynamicShape shape, IBaseImage image) : base(shape, image) { }
    public virtual void TriggerEffect()
    {
        DeleteEntity();
    }
    public void UpdateEffect()
    {
        if (Shape.Position.Y < -1)
        {
            DeleteEntity();
        }
    }
}