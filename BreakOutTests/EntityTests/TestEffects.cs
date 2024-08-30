using NUnit.Framework;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOut.EntityObjects;
using DIKUArcade.Physics;
using DIKUArcade.Math;


namespace BreakOutTests
{
    public class TestEffect
    {
        Effect? testsubject;

        [SetUp]
        public void Setup()
        {
            DIKUArcade.GUI.Window.CreateOpenGLContext();

            testsubject = new RocketEffect(new DynamicShape
            (new Vec2F(0.5f, 0.5f), new Vec2F(0.05f, 0.05f), new Vec2F(0.0f, -0.005f)));
        }

        /// <summary>
        /// Verifies that effect moves at a constant negative vertical speed.
        /// </summary>
        [Test]
        public void EffectMovingAtNegativeVerticalSpeed()
        {
            var init_y = testsubject?.Shape.Position.Y;

            testsubject?.Shape.Move();

            Assert.That(testsubject?.Shape.Position.Y < init_y);

            testsubject?.Shape.Move();

            Assert.That(testsubject?.Shape.Position.Y < init_y);
        }

        /// <summary>
        /// Verifies that effect disapears if it collides with the player.
        /// </summary>
        [Test]
        public void EffectCaughtByPlayer()
        {
            var playerShape = new DynamicShape(new Vec2F(0.4f, 0.1f), new Vec2F(0.2f, 0.03f));
            var playerImage = new Image(Path.Combine("Assets", "Images", "player.png"));
            var player = new Player(playerShape, playerImage);

            for (int x = 0; x < 100; x++)
            {
                if (CollisionDetection.Aabb((DynamicShape?)testsubject?.Shape, player?.Shape).Collision)
                {
                    testsubject?.TriggerEffect();
                }
                testsubject?.Shape.Move();
            }

            Assert.That(testsubject!.IsDeleted());
        }

        /// <summary>
        /// Verifies that effect is deleted if it is less then the lower window boundary
        /// </summary>
        [Test]
        public void EffectDeletedAtBottom()
        {

            for (int x = 0; x < 400; x++)
            {
                testsubject?.UpdateEffect();
                testsubject?.Shape.Move();
            }

            Assert.That(testsubject!.IsDeleted());
        }
    }
}