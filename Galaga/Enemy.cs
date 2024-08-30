using DIKUArcade.Entities;
using DIKUArcade.Graphics;
namespace Galaga;

public enum enemyState {
    Dead,
    Enraged,
    Alive,
}
public class Enemy : Entity {

    private int _hitpoints;
    private IBaseImage _enrageImage;
    
    private float _startX;
    private float _startY;
    
    public float StartX { get { return _startX; } }
    public float StartY { get { return _startY; } }
    
    public Enemy(DynamicShape shape, IBaseImage image, IBaseImage image2) : base(shape, image) {
        _enrageImage = image2;
        _hitpoints = 2;

        _startX = shape.Position.X;
        _startY = shape.Position.Y;
    }

    public enemyState State() {
        if (_hitpoints == 0) {
            return enemyState.Dead;
        }

        if (_hitpoints == 1) {
            return enemyState.Enraged;
        }

        return enemyState.Alive;
    }

    private void UpdateState() {
        switch (State()) {
            case enemyState.Dead:
                this.DeleteEntity();
                break;

            case enemyState.Enraged:
                this.Image = _enrageImage;
                //need to increase speed aswell.
                break;
        }
    }

    public void Hit() {
        _hitpoints--;
        UpdateState();
    }
}