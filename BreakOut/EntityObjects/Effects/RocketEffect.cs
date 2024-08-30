using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the Rocket Effect.
/// </summary>
public class RocketEffect : Effect
{
    public RocketEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "RocketPickUp.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotRocketPickup" });
    }
}