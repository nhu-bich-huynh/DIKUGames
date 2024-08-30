using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon of the DoubleSize effect.
/// </summary>
public class DoubleSizeEffect : Effect
{
    public DoubleSizeEffect(DynamicShape shape) : base(shape, new Image
        (Path.Combine("Assets", "Images", "BigPowerUp.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotDoubleSizePickup" });
    }
}