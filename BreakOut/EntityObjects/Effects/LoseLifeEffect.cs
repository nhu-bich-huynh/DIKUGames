using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the LoseLife effect
/// </summary>
public class LoseLifeEffect : Effect
{
    public LoseLifeEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "LoseLife.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotLoseLifePickup" });
    }
}