using Galaga;
using GalagaStates;

namespace GalagaTests
{
    [TestFixture]
    public class TestGameStateType
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestTransformStringToState()
        {
            var gameRunning = StateTransformer.TransformStringToState("GAME_RUNNING");
            var mainMenu = StateTransformer.TransformStringToState("MAIN_MENU");
            var gamePaused = StateTransformer.TransformStringToState("GAME_PAUSED");

            Assert.AreEqual(mainMenu, GameStateType.MainMenu);
            Assert.AreEqual(gameRunning, GameStateType.GameRunning);
            Assert.AreEqual(gamePaused, GameStateType.GamePaused);
        }
        
        [Test]
        public void TransformStateToString()
        {
            var gameRunning = StateTransformer.TransformStateToString(GameRunning.GetInstance());
            var mainMenu = StateTransformer.TransformStateToString(MainMenu.GetInstance());
            var gamePaused = StateTransformer.TransformStateToString(GamePaused.GetInstance());

            Assert.AreEqual(mainMenu,"MAIN_MENU");
            Assert.AreEqual(gameRunning,"GAME_RUNNING");
            Assert.AreEqual(gamePaused,"GAME_PAUSED");
        }
    }
}