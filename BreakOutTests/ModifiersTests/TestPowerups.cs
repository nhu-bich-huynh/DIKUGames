using BreakOut.Modifiers;
using NUnit.Framework;
using BreakOut;
using DIKUArcade.Events;
using BreakOut.EntityObjects;
using DIKUArcade.Math;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using BreakOutStates;

namespace BreakOutTests.ModifiersTests
{
    class TestPowerups
    {
        [SetUp]
        public void Setup()
        {
            DIKUArcade.GUI.Window.CreateOpenGLContext();
            BreakOutBus.GetBus().InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent,
                GameEventType.GraphicsEvent, GameEventType.StatusEvent
            });
        }

        [TearDown]
        public void TearDown()
        {
            BreakOutBus.SetBus(new GameEventBus());
        }

        /// <summary>
        /// Tests ExtraLife modifier by applying the modifier and checking if lives went up by 1
        /// </summary>
        [Test]
        public void TestExtraLife()
        {
            PlayerLives lives = new PlayerLives(new Vec2F(0.0f, 0.0f), new Vec2F(0.0f, 0.0f));
            int prev = lives.GetLives();

            Modifiers.Apply(GameFlag.ExtraLife);

            int curr = lives.GetLives();
            Assert.That(curr, Is.EqualTo(prev + 1));
        }

        /// <summary>
        /// Tests Wide modifier by applying the modifier and checking if extent went up by 2x
        /// </summary>
        [Test]
        public void TestWide()
        {
            Player player = new Player(new DynamicShape(new Vec2F(0.45f, 0.02f), new Vec2F(0.15f, 0.02f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

            float oldExt = player.Shape.Extent.X;

            Modifiers.Apply(GameFlag.Wide);

            Assert.That(player.Shape.Extent.X, Is.EqualTo(oldExt * 2));
        }

        /// <summary>
        /// Tests DoubleSize modifier by applying the modifier and checking if extent went up by 2x
        /// </summary>
        [Test]
        public void TestDoubleSize()
        {
            Ball ball = Ball.AtRandomAngle(0.0f, 0.0f, 1);
            Vec2F oldExt = ball.Shape.Extent;

            Modifiers.Apply(GameFlag.DoubleSize);

            Assert.That(ball.Shape.Extent.GetHashCode(), Is.EqualTo((oldExt * 2).GetHashCode()));
        }

        /// <summary>
        /// Tests PlayerSpeed modifier by applying the modifier and checking if speed went up by 2x
        /// </summary>
        [Test]
        public void TestPlayerSpeed()
        {
            Player player = new Player(new DynamicShape(new Vec2F(0.45f, 0.02f), new Vec2F(0.15f, 0.02f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));
            float oldSpeed = player.Speed;

            Modifiers.Apply(GameFlag.PlayerSpeed);

            Assert.That(player.Speed, Is.EqualTo(oldSpeed * 2));
        }

        /// <summary>
        /// Tests Rocket modifier by registering a rocket pickup and spacebarpress then checking 
        ///if rocket flag gets set after applying modifiers
        /// </summary>
        [Test]
        public void TestRocket()
        {
            GameRunning game = GameRunning.GetInstance();
            ModifierHandler modifierHandle = new ModifierHandler();

            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.StatusEvent,
                Message = "GotRocketPickup"
            });
            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.PlayerEvent,
                From = this,
                Message = "SpacebarPress"
            });

            BreakOutBus.GetBus().ProcessEventsSequentially();

            game.ApplyModifiers(modifierHandle.Flags);

            BreakOutBus.GetBus().ProcessEventsSequentially();

            bool hasRocketFlag = modifierHandle.Flags.HasFlag(GameFlag.Rocket);

            Assert.That(hasRocketFlag, Is.False);
        }
    }
}
