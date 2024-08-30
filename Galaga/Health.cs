using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga {
    public class Health {
        private int _health;
        private Text _display;
        public Health(Vec2F position, Vec2F extent) {
            _health = 3;
            _display = new Text(_health.ToString(), position, extent);
            _display.SetColor(System.Drawing.Color.White);
        }
        //When an enemy reaches the bottom of the screen,
        //1 point of health is lost. At zero health, the
        //game ends.
        public void LoseHealth() {
            _health--;
            _display.SetText(_health.ToString());
        }
        public void RenderHealth() {
            _display.RenderText();
        }
        public bool isDead() {
            if (_health <= 0) {
                return true;
            }

            return false;
        }
    }
}