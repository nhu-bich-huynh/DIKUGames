using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using Galaga.MovementStrategy;
using Galaga.Squadron;
using System;
using System.Collections.Generic;

namespace Galaga {
    public static class Director {
        private static float _speed = ConstValue.enemySpeed;
        private static Random _rand = new Random();
        private static ISquadron RandomSquadron() {
            switch (_rand.Next(0, 3)) {
                case 0:
                    return new LeftArrow();

                case 1:
                    return new RightArrow();

                case 2:
                    return new Line();

                default:
                    throw new Exception("Unhandled case");
            }
        }

        private static IMovementStrategy RandomStrategy() {
            switch (_rand.Next(0, 2)) {
                case 0:
                    return new ZigZagDown(_speed);

                case 1:
                    return new Down(_speed);

                default:
                    throw new Exception("Unhandled case");
            }
        }

        public static void StageClear(ref EntityContainer<Enemy> enemies, ref IMovementStrategy moveStrategy, List<Image> stride, List<Image> alternativeStride) {
            if (enemies.CountEntities() == 0) {
                var rndSquad = RandomSquadron();
                rndSquad.CreateEnemies(stride, alternativeStride);
                enemies = rndSquad.Enemies;
                _speed += ConstValue.enemySpeed;
                moveStrategy = RandomStrategy();

            }
        }
        public static bool AllEnemiesClear(EntityContainer<Enemy> enemies) {
            if (enemies.CountEntities() == 0) return true;
            else return false;
        }
    }
}
