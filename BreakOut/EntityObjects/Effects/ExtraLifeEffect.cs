using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the ExtraLife effect.
/// </summary>
public class ExtraLifeEffect : Effect
{
    public ExtraLifeEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "LifePickUp.png")))
    { }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotExtraLifePickup" });
    }
}