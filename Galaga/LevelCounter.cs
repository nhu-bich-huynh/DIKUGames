using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    public class LevelCounter {
        private int _currentLevel;
        private Text _display;
        public LevelCounter(Vec2F position, Vec2F extent) {
            _currentLevel = 0;
            _display = new Text($"Level: {_currentLevel.ToString()}", position, extent);
            _display.SetColor(System.Drawing.Color.White);
        }

        public void RenderLevelCounter() {
            _display.RenderText();
        }

        public void IncreaseLevel() {
            _currentLevel++;
            _display.SetText($"Level: {_currentLevel.ToString()}");
        }
    }
}