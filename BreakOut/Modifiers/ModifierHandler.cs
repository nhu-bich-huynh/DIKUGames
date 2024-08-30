using DIKUArcade.Events;
using DIKUArcade.Timers;

namespace BreakOut.Modifiers
{
    public class ModifierHandler : IGameEventProcessor
    {
        private GameFlag _flags;

        public ModifierHandler()
        {
            BreakOutBus.GetBus().Subscribe(GameEventType.StatusEvent, this);
        }

        public GameFlag Flags { get => _flags; }

        /// <summary>
        /// Resets the _flags property to be the empty flag
        /// </summary>
        public void Clear()
        {
            _flags = GameFlag.None;
        }


        /// <summary>
        /// Adds the supplied flag and registers a timed event to disable it after a set time.
        /// </summary>
        private void SetFlag(GameFlag flag)
        {
            BreakOutBus.GetBus().AddOrResetTimedEvent(
                new GameEvent
                {
                    EventType = GameEventType.StatusEvent,
                    Message = String.Format("Disable{0}", flag.ToString()),
                    Id = (uint)flag,
                },
                TimePeriod.NewMilliseconds(4000));

            _flags |= flag;
        }

        /// <summary>
        /// Listen for events to disable or enable modifiers either removing the flag or setting it.
        /// </summary>
        public void ProcessEvent(GameEvent gameEvent)
        {
            switch (gameEvent.Message)
            {
                //Powerups
                case "GotRocketPickup":
                    SetFlag(GameFlag.Rocket);
                    break;
                case "DisableRocket":
                    _flags &= ~GameFlag.Rocket;
                    break;
                case "GotExtraLifePickup":
                    SetFlag(GameFlag.ExtraLife);
                    break;
                case "DisableExtraLife":
                    _flags &= ~GameFlag.ExtraLife;
                    break;
                case "GotPlayerSpeedPickup":
                    SetFlag(GameFlag.PlayerSpeed);
                    break;
                case "DisablePlayerSpeed":
                    _flags &= ~GameFlag.PlayerSpeed;
                    break;
                case "GotWidePickup":
                    SetFlag(GameFlag.Wide);
                    break;
                case "DisableWide":
                    _flags &= ~GameFlag.Wide;
                    break;
                case "GotDoubleSizePickup":
                    SetFlag(GameFlag.DoubleSize);
                    break;
                case "DisableDoubleSize":
                    _flags &= ~GameFlag.DoubleSize;
                    break;
                //Hazards
                case "GotLoseLifePickup":
                    SetFlag(GameFlag.LoseLife);
                    break;
                case "DisableLoseLife":
                    _flags &= ~GameFlag.LoseLife;
                    break;
                case "GotFogOfWarPickup":
                    SetFlag(GameFlag.FogOfWar);
                    break;
                case "DisableFogOfWar":
                    _flags &= ~GameFlag.FogOfWar;
                    break;
            }
        }
    }
}
