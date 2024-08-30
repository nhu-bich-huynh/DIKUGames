using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;
using DIKUArcade.Events;

namespace Galaga {
    public class Player {
        private Entity entity;

        private DynamicShape shape;
        private float moveLeft = 0f;
        private float moveRight = 0f;
        private const float MOVEMENT_SPEED = 0.01f;
        public Player(DynamicShape shape, IBaseImage image) {
            entity = new Entity(shape, image);
            this.shape = shape;
        }
        public void Render() {
            this.entity.RenderEntity();
        }

        public void Move() {
            var oldPos = this.shape.Position;

            this.shape.Move();

            // Maybe change this in the future to place at edge of screen instead of snapping back to previous position.
            if (this.shape.Position.X > 1 || this.shape.Position.X < 0) {
                this.shape.Position = oldPos;
            }
            else if (this.shape.Position.Y > 1 || this.shape.Position.Y < 0) {
                this.shape.Position = oldPos;
            };
        }

        // Getter to get for constructing playershot in game.cs
        public Vec2F GetPosition() {
            return shape.Position;
        }

        private void SetMoveLeft(bool val) {
            if (val) {
                moveLeft = -MOVEMENT_SPEED;
            } 
            else {
                moveLeft = 0f;
            }
            UpdateDirection();
        }

        private void SetMoveRight(bool val) {
            if (val) {
                moveRight = MOVEMENT_SPEED;
            } 
            else {
                moveRight = 0f;  
            }
            UpdateDirection();
        }

        private void UpdateDirection() {
            this.shape.Direction.X = moveLeft + moveRight;
        }

        public void ProcessEvent(GameEvent gameEvent) {
            if (gameEvent.EventType == GameEventType.PlayerEvent) {
                switch (gameEvent.Message) {
                    case "MoveLeftPress":
                        this.SetMoveLeft(true);
                    break;

                    case "MoveRightPress":
                        this.SetMoveRight(true);
                    break;

                    case "MoveLeftRelease":
                        this.SetMoveLeft(false);
                    break;

                    case "MoveRightRelease":
                        this.SetMoveRight(false);
                    break;
                }
            }
        }
    }
}