using DIKUArcade.Entities;
using DIKUArcade.Events;
using DIKUArcade.Graphics;

namespace BreakOut.EntityObjects
{
    /// <summary>
    /// This class represents the default block type with no special properties or abilities.
    /// </summary>
    public class Block : Entity
    {
        protected int value;
        protected int health;


        public Block(StationaryShape shape, IBaseImage image) : base(shape, image)
        {
            value = 1;
            health = 1;
        }

        public Block(int value, int health, StationaryShape shape, IBaseImage image) : base(shape, image)
        {
            this.value = value;
            this.health = health;
        }

        /// <summary>
        /// Assesses and returns the current state of the block
        /// </summary>
        public blockState State()
        {
            if (health == 0)
            {
                return blockState.Dead;
            }
            if (health == 1)
            {
                return blockState.Damaged;
            }

            return blockState.Alive;
        }

        protected virtual void UpdateState()
        {
            if (State() == blockState.Dead)
            {
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.StatusEvent,
                    From = this,
                    Message = "BlockDestroyed",
                    IntArg1 = value,
                });
                DeleteEntity();
            }
        }

        /// <summary>
        /// Runs the logic for when the block is hit by the ball or a rocket.
        /// </summary>
        public virtual void Hit()
        {
            health--;
            UpdateState();
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetValue()
        {
            return value;
        }
    }
}
