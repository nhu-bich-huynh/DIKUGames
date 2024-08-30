using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the PlayerSpeed Effect.
/// </summary>
public class PlayerSpeedEffect : Effect
{
    public PlayerSpeedEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "SpeedPickUp.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotPlayerSpeedPickup" });
    }
}