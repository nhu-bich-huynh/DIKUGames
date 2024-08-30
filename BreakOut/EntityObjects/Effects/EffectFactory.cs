using DIKUArcade.Entities;
using DIKUArcade.Events;

namespace BreakOut.EntityObjects;

/// <summary>
/// Static class used for spawning either a random Hazard or PowerUp.
/// </summary>
static public class EffectFactory
{
    static private void CreateEffect(Effect effect)
    {
        BreakOutBus.GetBus().RegisterEvent(new GameEvent
        { EventType = GameEventType.GraphicsEvent, Message = "EffectCreated", ObjectArg1 = effect });
    }

    /// <summary>
    /// Spawns a random PowerUp at a given position.
    /// </summary>
    static public void CreateRandomPowerUp(DynamicShape shape)
    {
        int random = BreakoutRandom.GetRand().Next(5);
        switch (random)
        {
            case 0:
                CreateEffect(new RocketEffect(shape));
                break;
            case 1:
                CreateEffect(new ExtraLifeEffect(shape));
                break;
            case 2:
                CreateEffect(new WideEffect(shape));
                break;
            case 3:
                CreateEffect(new PlayerSpeedEffect(shape));
                break;
            case 4:
                CreateEffect(new DoubleSizeEffect(shape));
                break;
            default: throw new Exception("ERROR IN EFFECT FACTORY.CS!");
        }
    }

    /// <summary>
    /// Spawns a random hazard at a given position.
    /// </summary>
    static public void CreateRandomHazard(DynamicShape shape)
    {
        int random = BreakoutRandom.GetRand().Next(2);
        switch (random)
        {
            case 0:
                CreateEffect(new LoseLifeEffect(shape));
                break;
            case 1:
                CreateEffect(new FogOfWarEffect(shape));
                break;
            default: throw new Exception("ERROR IN EFFECT FACTORY.CS!");
        }
    }
}