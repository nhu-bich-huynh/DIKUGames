using DIKUArcade.Entities;
using DIKUArcade.Math;

using System;

namespace Galaga.MovementStrategy;

public class ZigZagDown : IMovementStrategy {
    private float _moveSpeed;
    private float _p;
    private float _a;

    public ZigZagDown() {
        _moveSpeed = ConstValue.enemySpeed;
        _p = 0.045f;
        _a = 0.05f;
    }

    public ZigZagDown(float moveSpeed) {
        _moveSpeed = moveSpeed;
        _p = 0.045f;
        _a = 0.05f;
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

        enemy.Shape.Position.Y += -speed;
        enemy.Shape.Position.X = (float)(enemy.StartX + _a * Math.Sin((2 * Math.PI * (enemy.StartY - enemy.Shape.Position.Y)) / _p));
    }
}