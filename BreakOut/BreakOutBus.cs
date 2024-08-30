using DIKUArcade.Events;
namespace BreakOut;

/// <summary>
/// Represents the game's event bus, which mediates communication between classes.
/// </summary>
public static class BreakOutBus
{
    private static GameEventBus? eventBus;
    public static GameEventBus GetBus()
    {
        return BreakOutBus.eventBus ??= new GameEventBus();
    }

    public static void SetBus(GameEventBus bus)
    {
        eventBus = bus;
    }
}