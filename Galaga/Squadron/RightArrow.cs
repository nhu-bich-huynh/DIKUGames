using System.IO;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Math;
using DIKUArcade.Events;
using DIKUArcade.Input;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Physics;
using System.Collections.Generic;

namespace Galaga.Squadron;

public class RightArrow : ISquadron {
    public EntityContainer<Enemy> Enemies { get { return _enemies; } }
    private EntityContainer<Enemy> _enemies;

    public int MaxEnemies { get { return _maxEnemies; } }
    private int _maxEnemies;

    public RightArrow() {
        _maxEnemies = 6;
        _enemies = new EntityContainer<Enemy>(_maxEnemies);
    }

    public void CreateEnemies(List<Image> enemyStride, List<Image> alternativeEnemyStride) {
        for (int i = 0; i < MaxEnemies/2; i++) {
            for (int j = 0; j < MaxEnemies/2-i; j++) {
                _enemies.AddEntity(new Enemy(
                    new DynamicShape(new Vec2F(0.5f + (float)i * 0.1f, 0.9f - (float)j * 0.1f), new Vec2F(0.1f, 0.1f)),
                    new ImageStride(80, enemyStride),
                    new ImageStride(80, alternativeEnemyStride)
                ));
            }
        }
    }
}