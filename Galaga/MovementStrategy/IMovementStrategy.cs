using DIKUArcade.Entities;
using DIKUArcade.Math;

using System;

namespace Galaga.MovementStrategy {
    public interface IMovementStrategy {
        void MoveEnemy(Enemy enemy);
        void MoveEnemies(EntityContainer<Enemy> enemies);
    }
}