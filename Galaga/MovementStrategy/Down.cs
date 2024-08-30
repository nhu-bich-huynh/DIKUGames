using DIKUArcade.Entities;
using DIKUArcade.Math;

using System;

namespace Galaga.MovementStrategy;


public class Down : IMovementStrategy {
    private float _moveSpeed;

    public Down() {
        _moveSpeed = ConstValue.enemySpeed;
    }

    public Down(float moveSpeed) {
        _moveSpeed = moveSpeed;
    }

    public void MoveEnemies(EntityContainer<Enemy> enemies) {
        foreach (Enemy enemy in enemies) {
            MoveEnemy(enemy);
        }
    }

    public void MoveEnemy(Enemy enemy) {
        var speed = _moveSpeed;

        if (enemy.State() == enemyState.Enraged) {
            speed = 2 * speed;
        }

        enemy.Shape.MoveY(-speed);
    }
}