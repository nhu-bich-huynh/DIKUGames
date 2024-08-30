using BreakOut.EntityObjects;
using NUnit.Framework;
using DIKUArcade.Math;
using BreakOut.Modifiers;
using BreakOut;
using DIKUArcade.Events;

namespace BreakOutTests.ModifiersTests
{
    class HazardTests
    {
        [SetUp]
        public void Setup()
        {
            DIKUArcade.GUI.Window.CreateOpenGLContext();

            BreakOutBus.GetBus().InitializeEventBus(new List<GameEventType>
                { GameEventType.InputEvent, GameEventType.PlayerEvent, GameEventType.GameStateEvent,
                GameEventType.GraphicsEvent, GameEventType.StatusEvent });
        }

        [TearDown]
        public void TearDown()
        {
            BreakOutBus.SetBus(new GameEventBus());
        }

        /// <summary>
        /// Tests LoseLife modifier by applying the modifier and checking if lives went down by 1
        /// </summary>
        [Test]
        public void TestLoseLife()
        {
            PlayerLives lives = new PlayerLives(new Vec2F(0.0f, 0.0f), new Vec2F(0.0f, 0.0f));
            int prev = lives.GetLives();

            Modifiers.Apply(GameFlag.LoseLife);

            int curr = lives.GetLives();
            Assert.That(curr, Is.EqualTo(prev - 1));
        }

        /// <summary>
        /// Tests FogOfWar modifier by applying the modifier and checking if the BlockFormation 
        /// property shown is false
        /// </summary>
        [Test]
        public void TestFogOfWar()
        {
            Level level = LevelLoader.LoadLevel(Path.Combine("Assets", "Levels", "alevel.txt"));
            BlockFormation blocks = new BlockFormation(level);

            Modifiers.Apply(GameFlag.FogOfWar);

            Assert.That(blocks.shown, Is.False);
        }
    }
}
