using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Represents the visual icon for the FogOfWar Effect
/// </summary>
public class FogOfWarEffect : Effect
{
    public FogOfWarEffect(DynamicShape shape) : base(shape, new Image
    (Path.Combine("Assets", "Images", "Ghost.png")))
    {
    }
    public override void TriggerEffect()
    {
        base.TriggerEffect();
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.StatusEvent, Message = "GotFogOfWarPickup" });
    }
}