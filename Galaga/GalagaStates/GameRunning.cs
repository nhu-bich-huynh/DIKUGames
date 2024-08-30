using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using System.IO;
using DIKUArcade.Input;
using Galaga;
using DIKUArcade;
using DIKUArcade.GUI;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using System.Collections.Generic;
using Galaga.MovementStrategy;
using Galaga.Squadron;
using GalagaStates;
using Galaga.GalagaStates;

namespace GalagaStates;


public class GameRunning : IGameState, IGameEventProcessor {
    private LevelCounter levelCounter;
    private Health playerHealth;
    private const int EXPLOSION_LENGTH_MS = 500;
    private IMovementStrategy moveStrat;
    private AnimationContainer enemyExplosions;
    private List<Image> explosionStrides;
    private static GameRunning instance = null;
    private Player player;
    private bool gamestate;
    private EntityContainer<Enemy> enemies;
    private ISquadron squad;
    private List<Image> enemyStridesBlue;
    private List<Image> enemyStridesRed;
    private IBaseImage playerShotImage;
    private EntityContainer<PlayerShot> playerShots;
    private Entity backGroundImage = new Entity(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)), 
        new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
    public static GameRunning GetInstance() {
        if (GameRunning.instance == null) {
            GameRunning.instance = new GameRunning();
            GameRunning.instance.ResetState();
        }
        return GameRunning.instance;
    }
    public void ResetState() {
        enemyStridesBlue = ImageStride.CreateStrides(4, Path.Combine("Assets", "Images", "BlueMonster.png"));
        enemyStridesRed = ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "RedMonster.png"));

        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

        squad = new RightArrow();
        squad.CreateEnemies(enemyStridesBlue, enemyStridesRed);

        enemies = squad.Enemies;

        playerShots = new EntityContainer<PlayerShot>();

        playerShotImage = new Image(Path.Combine("Assets", "Images", "BulletRed2.png"));

        enemyExplosions = new AnimationContainer(squad.MaxEnemies);
        explosionStrides = ImageStride.CreateStrides(8, Path.Combine("Assets", "Images", "Explosion.png"));

        moveStrat = new ZigZagDown();

        playerHealth = new Health(new Vec2F(0.03f, -0.3f), new Vec2F(0.4f, 0.4f));

        levelCounter = new LevelCounter(new Vec2F(0.7f, -0.3f), new Vec2F(0.4f, 0.4f));

        gamestate = true;

        GalagaBus.GetBus().Subscribe(GameEventType.InputEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.WindowEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
        GalagaBus.GetBus().Subscribe(GameEventType.GameStateEvent, this);
        }

        private void KeyPress(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Left:
                GalagaBus.GetBus().RegisterEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveLeftPress" });
                break;
            case KeyboardKey.Right:
                GalagaBus.GetBus().RegisterEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightPress" });
                break;
            case KeyboardKey.Space:
                playerShots.AddEntity(new PlayerShot(player.GetPosition(), playerShotImage));
                break;
            case KeyboardKey.Escape:
                GalagaBus.GetBus().RegisterEvent(new GameEvent{EventType = GameEventType.GameStateEvent, From = this, Message = "GAME_PAUSED"});
                break;
        }
    }

    private void KeyRelease(KeyboardKey key) {
        switch (key) {
            case KeyboardKey.Left:
                GalagaBus.GetBus().RegisterEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveLeftRelease" });
                break;
            case KeyboardKey.Right:
                GalagaBus.GetBus().RegisterEvent(new GameEvent { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightRelease" });
                break;
        }
    }
    public void AddExplosion(Vec2F position, Vec2F extent) {
        enemyExplosions.AddAnimation(new StationaryShape(position, extent), EXPLOSION_LENGTH_MS, new ImageStride(EXPLOSION_LENGTH_MS/8, explosionStrides));
    }
    private void IterateShots() {
        playerShots.Iterate(shot => {
            shot.Move();
            if (shot.Shape.Position.X > 1 || shot.Shape.Position.X < 0
                || shot.Shape.Position.Y > 1 || shot.Shape.Position.Y < 0) {
                    shot.DeleteEntity();
                }
            else {
                enemies.Iterate(Enemy => {
                    if (CollisionDetection.Aabb(shot.Shape.AsDynamicShape(), Enemy.Shape).Collision) {
                        AddExplosion(Enemy.Shape.Position, Enemy.Shape.Extent);
                        Enemy.Hit();
                        shot.DeleteEntity();
                    }
                });
            }
        });
    }
    public void ProcessEvent(GameEvent gameEvent) {
        switch (gameEvent.EventType) {
            case GameEventType.PlayerEvent:
                player.ProcessEvent(gameEvent);
            break;
        }
    }

    public void HandleKeyEvent(KeyboardAction keyboardAction, KeyboardKey keyboardKey) {
        switch (keyboardAction) {
            case (KeyboardAction.KeyPress):
                KeyPress(keyboardKey);
                break;
            case (KeyboardAction.KeyRelease):
                KeyRelease(keyboardKey);
                break;
        }
    }

    public void RenderState() {
        if (gamestate) {
            backGroundImage.RenderEntity();
            player.Render();
            enemies.RenderEntities();
            playerShots.RenderEntities();
            enemyExplosions.RenderAnimations();
            playerHealth.RenderHealth();
        }
        levelCounter.RenderLevelCounter();
    }
    public void UpdateState() {
        if (gamestate) {
            player.Move();
            IterateShots();
            moveStrat.MoveEnemies(enemies);
            if (Director.AllEnemiesClear(enemies)) levelCounter.IncreaseLevel();
            Director.StageClear(ref enemies, ref moveStrat, enemyStridesBlue, enemyStridesRed);
            gamestate = !GameOver.IsGameOver(enemies, playerHealth);
        }
    }
}