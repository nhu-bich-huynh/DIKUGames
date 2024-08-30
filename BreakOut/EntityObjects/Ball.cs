using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
namespace BreakOut.EntityObjects
{
    using Modifiers;
    /// <summary>
    /// The Ball class represents the ball being bounced around on the screen and controls 
    /// its movement and position.
    /// </summary>
    public class Ball : Entity, IFlagHandler
    {
        private readonly float speed;
        private bool isDoubleSize;

        public Ball(DynamicShape shape, IBaseImage image, float speed) : base(shape, image)
        {
            this.speed = speed;
            this.isDoubleSize = false;

            Modifiers.Subscribe(this);
        }

        /// <summary>
        /// Spawns a ball with a given speed and position, and sets it to move in a random direction.
        /// </summary>
        public static Ball AtRandomAngle(float x, float y, float speed)
        {
            double randAng = BreakoutRandom.GetRand().NextDouble() * Math.PI * 0.5 + Math.PI * 0.25;

            Vec2F dir = new((float)Math.Cos(randAng), (float)Math.Sin(randAng));
            dir = dir / (float)dir.Length() * speed;

            Vec2F ext = new(0.05f, 0.05f);
            DynamicShape shape = new(new Vec2F(x, y) - ext / 2, ext, dir);

            Image image = new(Path.Combine("Assets", "Images", "ball.png"));

            return new Ball(shape, image, speed);
        }

        /// <summary>
        /// Moves the ball within the bounderies of the screen or deletes it if it has reached 
        /// the bottom of the screen.
        /// </summary>
        public void Move()
        {
            DynamicShape dynShape = (DynamicShape)Shape;
            Vec2F oldPos = dynShape.Position;

            dynShape.Move();

            if (dynShape.Position.X + dynShape.Extent.X > 1.0f || dynShape.Position.X < 0.0f)
            {
                dynShape.Position = oldPos;
                dynShape.Direction.X *= -1;
            }

            if (dynShape.Position.Y + dynShape.Extent.Y > 1.0f)
            {
                dynShape.Position = oldPos;
                dynShape.Direction.Y *= -1;
            }

            if (dynShape.Position.Y < 0.0f)
            {
                this.DeleteEntity();
            }
        }

        /// <summary>
        /// Bounces the ball by reversing its vertical momentum.
        /// </summary>
        public void Bounce()
        {
            DynamicShape dynShape = (DynamicShape)Shape;

            dynShape.Direction.Y *= -1;
        }

        /// <summary>
        /// Bounces the ball off a block, hitting it.
        /// </summary>
        public void Bounce(Block block)
        {
            Bounce();
            block.Hit();
        }

        /// <summary>
        /// Bounces the ball off a player. This changes the angle of the ball depending on where on 
        /// the player the ball was bounced from.
        /// </summary>
        public void PlayerBounce(Player? player)
        {
            if (player == null)
            {
                return;
            }

            DynamicShape dynShape = (DynamicShape)Shape;

            float playerMidPos = player.Shape.Position.X + player.Shape.Extent.X / 2.0f;
            float ballMidPos = dynShape.Position.X + dynShape.Extent.X / 2.0f;
            float distFromMid = ballMidPos - playerMidPos;

            dynShape.Direction.X = 0.05f * (distFromMid / player.Shape.Extent.X);
            dynShape.Direction.Y *= -1;

            Vec2F norm = dynShape.Direction / (float)dynShape.Direction.Length();

            if (Math.Abs(norm.X) > 0.7f)
            {
                if (norm.X > 0)
                {
                    norm.X = 0.7f;
                }
                else
                {
                    norm.X = -0.7f;
                }

                if (norm.Y > 0)
                {
                    norm.Y = 0.7f;
                }
                else
                {
                    norm.Y = -0.7f;
                }
            }

            dynShape.Direction.X = norm.X * speed;
            dynShape.Direction.Y = norm.Y * speed;
        }

        public float GetSpeed()
        {
            return speed;
        }

        /// <summary>
        /// Applies a modifier from a given effect to the ball.
        /// </summary>
        public void ApplyModifiers(GameFlag flag)
        {
            if (Modifiers.Added(flag).HasFlag(GameFlag.DoubleSize))
            {
                Shape.Position -= Shape.Extent;
                Shape.Extent *= 2;
                isDoubleSize = true;
            }

            if (Modifiers.Removed(flag).HasFlag(GameFlag.DoubleSize) && isDoubleSize)
            {
                Shape.Extent /= 2;
                Shape.Position += Shape.Extent;
                isDoubleSize = false;
            }
        }
    }
}
