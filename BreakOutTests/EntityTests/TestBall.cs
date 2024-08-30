using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;
using DIKUArcade.Math;


namespace BreakOutTests
{
    public class TestBall
    {

        [SetUp]
        public void Setup()
        {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
        }

        /// <summary>
        /// Verifies that when a ball leaves the screen it will be deleted
        /// </summary>
        [Test]
        public void BallDeletedAtBottom()
        {
            var ball = Ball.AtRandomAngle(0.5f, -0.5f, 0.0f);

            ball.Move();

            Assert.That(ball.IsDeleted(), Is.True);
        }

        /// <summary>
        /// Verifies that the ball must only be able to leave the screen through the bottom
        /// </summary>
        [Test]
        public void BallOnlyLeavingThroughBottom() {
            var ball = Ball.AtRandomAngle(0.5f, 0.9f, 0.02f);
            var player = new Player(new DynamicShape(new Vec2F(0.4f, 0.1f), new Vec2F(0.2f, 0.03f)),
                new Image(Path.Combine("Assets", "Images", "player.png")));

            ball.PlayerBounce(player);
            ball.Move();
            ball.Move();
            ball.Move();
            ball.Move();
            ball.Move();

            Assert.Multiple(() => {
                Assert.That(ball.Shape.Position.Y, Is.GreaterThan(0));
                Assert.That(ball.Shape.Position.Y, Is.LessThan(1));
            });
        }

        /// <summary>
        /// Verifies that if the ball collides with a block, said block is 'hit'.
        /// </summary>
        [Test]
        public void BallCollidesWithBlock()
        {
            var block = new Block(new StationaryShape(0.5f, 0.5f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "grey-block.png")));
            var ball = Ball.AtRandomAngle(0.5f, 0.5f, 0.02f);

            ball.Bounce(block);

            Assert.That(block.State(), Is.EqualTo(blockState.Dead));
        }

        /// <summary>
        /// Verifies that all balls must always move at the same speed.
        /// </summary>
        [Test]
        public void BallMovingAtConstantSpeed()
        {
            var block = new Block(new StationaryShape(0.5f, 0.5f, 1.0f, 1.0f),
                new Image(Path.Combine("Assets", "Images", "grey-block.png")));
            var ball = Ball.AtRandomAngle(0.5f, 0.5f, 0.02f);
            var player = new Player(new DynamicShape(new Vec2F(0.4f, 0.1f), new Vec2F(0.2f, 0.03f)),
                new Image(Path.Combine("Assets", "Images", "player.png")));
            float ballspeed = ball.GetSpeed();

            for (int x = 0; x < 10; x++)
            {
                Assert.That(ball.GetSpeed(), Is.EqualTo(ballspeed));
                ball.Move();
                Assert.That(ball.GetSpeed(), Is.EqualTo(ballspeed));
                ball.Bounce();
                Assert.That(ball.GetSpeed(), Is.EqualTo(ballspeed));
                ball.Bounce(block);
                Assert.That(ball.GetSpeed(), Is.EqualTo(ballspeed));
                ball.PlayerBounce(player);
                Assert.That(ball.GetSpeed(), Is.EqualTo(ballspeed));
            }
        }

        /// <summary>
        /// Verifies that ball always launches with a positive upwards direction
        /// and it always launches more vertical than horizontal.
        /// </summary>
        [Test]
        public void BallMovingUpwards()
        {
            var ball = Ball.AtRandomAngle(0.5f, 0.5f, 0.02f);
            var init_x = ball.Shape.Position.X;
            var init_y = ball.Shape.Position.Y;

            ball.Move();
            ball.Move();

            var new_x = ball.Shape.Position.X;
            var new_y = ball.Shape.Position.Y;

            var delta_x = new_x - init_x;
            var delta_y = new_y - init_y;

            Assert.That(delta_y, Is.GreaterThan(delta_x));
        }
    }
}