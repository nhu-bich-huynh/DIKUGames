using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the Wide effect.
/// </summary>
public class WideEffect : Effect
{
    public WideEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "WidePowerUp.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotWidePickup" });
    }
}