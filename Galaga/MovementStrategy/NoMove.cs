using DIKUArcade.Entities;
using DIKUArcade.Math;

using System;

namespace Galaga.MovementStrategy;

public class NoMove : IMovementStrategy {
    public void MoveEnemies(EntityContainer<Enemy> enemies) {
    }

    public void MoveEnemy(Enemy enemy) {
    }
}