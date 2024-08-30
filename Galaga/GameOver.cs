using DIKUArcade.Entities;

namespace Galaga {
    public class GameOver {
        public static bool IsGameOver(EntityContainer<Enemy> enemies, Health health) {
            enemies.Iterate(enemy => {
                if (enemy.Shape.Position.Y <= 0.08) {
                    enemy.DeleteEntity();
                    health.LoseHealth();
                }
            });
            
            if (health.isDead()) {
                return true;
            }

            return false;
        }

    }
}
