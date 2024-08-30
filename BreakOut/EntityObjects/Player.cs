using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Timers;

namespace BreakOut.EntityObjects
{
    using Modifiers;

    /// <summary>
    /// The Player class represents the movable Player object and controls the movement
    /// and position of the player.
    /// </summary>
    public class Player : Entity, IGameEventProcessor, IFlagHandler
    {
        private float moveLeft;
        private float moveRight;
        private float speed;
        private bool shoot;
        private bool isWide;

        public bool Shoot { get => shoot; }
        public float Speed { get => speed; }

        /// <summary>
        /// Constructs a Player object.
        /// </summary>
        /// <param name="shape">The dynamic shape of the player.</param>
        /// <param name="image">The image representing the player.</param>
        public Player(DynamicShape shape, IBaseImage image) : base(shape, image)
        {
            moveLeft = 0f;
            moveRight = 0f;
            speed = 0.02f;
            shoot = false;
            isWide = false;

            BreakOutBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
            Modifiers.Subscribe(this);
        }
        /// <summary>
        /// The Render method renders the Player to the screen
        /// </summary>
        public void Render()
        {
            RenderEntity();
        }

        /// <summary>
        /// The Move method moves the player, but not beyond the bounderies of the screen
        /// </summary>
        public void Move()
        {
            Vec2F oldPos = Shape.Position;

            Shape.Move();

            if (Shape.Position.X > 1 - Shape.Extent.X || Shape.Position.X < 0)
            {
                Shape.Position = oldPos;
            }
            else if (Shape.Position.Y > 1 || Shape.Position.Y < 0)
            {
                Shape.Position = oldPos;
            };
        }

        /// <summary>
        /// Gets the current position of the player.
        /// </summary>
        /// <returns>The position vector of the player.</returns>
        public Vec2F GetPosition()
        {
            return Shape.Position;
        }

        private void SetMoveLeft(bool val)
        {
            if (val)
            {
                moveLeft = -speed;
            }
            else
            {
                moveLeft = 0f;
            }
            UpdateDirection();
        }

        private void SetMoveRight(bool val)
        {
            if (val)
            {
                moveRight = speed;
            }
            else
            {
                moveRight = 0f;
            }
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            ((DynamicShape)Shape).Direction.X = moveLeft + moveRight;
        }

        /// <summary>
        /// Resets the shoot flag of the player.
        /// </summary>
        public void ResetShoot()
        {
            shoot = false;
        }

        /// <summary>
        /// Processes game events triggered by the player.
        /// </summary>
        /// <param name="gameEvent">The game event to process.</param>
        public void ProcessEvent(GameEvent gameEvent)
        {
            if (gameEvent.Message == "TestEffect") this.DeleteEntity();

            switch (gameEvent.Message)
            {
                case "MoveLeftPress":
                    this.SetMoveLeft(true);
                    break;

                case "MoveRightPress":
                    this.SetMoveRight(true);
                    break;

                case "MoveLeftRelease":
                    this.SetMoveLeft(false);
                    break;

                case "MoveRightRelease":
                    this.SetMoveRight(false);
                    break;
                case "SpacebarPress":
                    shoot = true;
                    break;

            }
        }

        /// <summary>
        /// Applies a modifier from a certain effect to the player.
        /// </summary>
        public void ApplyModifiers(GameFlag flag)
        {
            GameFlag added = Modifiers.Added(flag);
            GameFlag removed = Modifiers.Removed(flag);

            if (added.HasFlag(GameFlag.PlayerSpeed))
            {
                speed *= 2;

                BreakOutBus.GetBus().AddOrResetTimedEvent(
                    new GameEvent
                    {
                        EventType = GameEventType.StatusEvent,
                        Message = "DisablePlayerSpeed",
                    },
                    TimePeriod.NewMilliseconds(2000));
            }
            if (removed.HasFlag(GameFlag.PlayerSpeed))
            {
                speed /= 2;
            }

            if (added.HasFlag(GameFlag.Wide))
            {
                Shape.Position.X = Shape.Position.X - Shape.Extent.X;
                Shape.Extent.X = Shape.Extent.X * 2;

                if (Shape.Position.X < 0f)
                {
                    Shape.Position.X = 0f;
                }

                isWide = true;
            }

            if (removed.HasFlag(GameFlag.Wide) && isWide)
            {
                Shape.Extent.X = Shape.Extent.X / 2;
                Shape.Position.X = Shape.Position.X + Shape.Extent.X;
                isWide = false;
            }
        }
    }
}