using DIKUArcade.State;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.Input;
using DIKUArcade.Events;
using DIKUArcade.Physics;
using BreakOut;
using BreakOut.EntityObjects;
using BreakOut.Modifiers;
using DIKUArcade.Timers;

namespace BreakOutStates;

/// <summary>
/// Represents the state of the game while it is running an active game.
/// </summary>
public class GameRunning : IGameState, IFlagHandler, IGameEventProcessor
{
    private static GameRunning? instance;
    private Player? player;
    private Points? playerRewards;
    private PlayerLives? playerLives;
    private bool isActive;
    private readonly Entity backGroundImage = new(new DynamicShape(
        new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
        new Image(Path.Combine("Assets", "Images", "SpaceBackground.png")));
    private BlockFormation? blocks;
    private readonly string levelFolderPath = Path.Combine("Assets", "Levels");
    private string[]? levels;
    private Level? level;
    private int activeLevel;
    private int amountOfLevels;
    private EntityContainer<Ball>? balls;
    private EntityContainer<Effect>? effects;
    private EntityContainer<Rocket>? rockets;
    private ModifierHandler? modifierHandler;
    private int ballsCount;
    private LevelTimeOut? time;
    public static GameRunning GetInstance()
    {
        StaticTimer.ResumeTimer();

        if (GameRunning.instance == null)
        {
            GameRunning.instance = new GameRunning();
            GameRunning.instance.ResetState();
            GameRunning.instance.StarupState();
        }
        return GameRunning.instance;
    }

    public void StarupState()
    {
        BreakOutBus.GetBus().Subscribe(GameEventType.GraphicsEvent, GameRunning.instance);
    }

    /// <summary>
    /// Starts an entirely new game.
    /// </summary>
    public void ResetState()
    {
        Modifiers.Clear();
        Modifiers.Subscribe(this);

        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.02f), new Vec2F(0.15f, 0.02f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

        isActive = true;

        levels = Directory.GetFiles(levelFolderPath, "*.txt");
        amountOfLevels = levels.Length;
        activeLevel = 0;

        level = LevelLoader.LoadLevel(levels[activeLevel]);

        blocks = new BlockFormation(level);

        balls = new EntityContainer<Ball>();
        balls.AddEntity(Ball.AtRandomAngle(0.5f, 0.2f, 0.02f));

        rockets = new EntityContainer<Rocket>();

        playerRewards = new Points(new Vec2F(0.03f, 0.7f), new Vec2F(0.3f, 0.3f));

        effects = new EntityContainer<Effect>();

        modifierHandler = new ModifierHandler();

        playerLives = new PlayerLives(new Vec2F(0.75f, 0.7f), new Vec2F(0.3f, 0.3f));

        ballsCount = balls.CountEntities();

        time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));
    }

    /// <summary>
    /// Resets everything needed for changing between levels, but not for a new game.
    /// </summary>
    private void ResetLevel()
    {
        player = new Player(
            new DynamicShape(new Vec2F(0.45f, 0.02f), new Vec2F(0.15f, 0.02f)),
            new Image(Path.Combine("Assets", "Images", "Player.png")));

        level = LevelLoader.LoadLevel(levels?[activeLevel]);

        blocks = new BlockFormation(level);

        if (balls != null)
        {
            balls.ClearContainer();
            balls.AddEntity(Ball.AtRandomAngle(0.5f, 0.2f, 0.02f));
            ballsCount = balls.CountEntities();
        }

        rockets?.ClearContainer();

        effects?.ClearContainer();

        modifierHandler?.Clear();

        time = new LevelTimeOut(level, new Vec2F(0.45f, 0.7f), new Vec2F(0.3f, 0.3f));
    }

    /// <summary>
    /// Switches to the next possible level, or wins the game if no more possible levels remain.
    /// </summary>
    public void NextLevel()
    {
        if (activeLevel < amountOfLevels - 1)
        {
            activeLevel++;
            ResetLevel();
        }
        else
        {
            int points = -1;
            if (playerRewards != null)
            {
                points = playerRewards.GetPoints();
            }

            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.StatusEvent,
                From = this,
                Message = "Game Winning Score",
                IntArg1 = points,
            });
            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.GameStateEvent,
                From = this,
                Message = "GAME_WINNING",
            });
        }
    }

    /// <summary>
    /// Assesses whether the level has been cleared, and switches to the next level if it has.
    /// </summary>
    private void CheckLevelClear()
    {
        if (blocks?.Container.CountEntities() <= blocks?.UnbreakableBlocksCount()) NextLevel();
    }

    /// <summary>
    /// Updates and moves all active effects.
    /// </summary>
    private void IterateEffects()
    {
        effects?.Iterate(effect =>
        {
            if (CollisionDetection.Aabb((DynamicShape)effect.Shape, player?.Shape).Collision)
            {
                effect.TriggerEffect();
            }
            effect.UpdateEffect();
            effect.Shape.Move();
        });
    }

    /// <summary>
    /// Updates and moves all active balls.
    /// </summary>
    private void IterateBalls()
    {
        balls?.Iterate(ball =>
        {
            ball.Move();
            if (CollisionDetection.Aabb((DynamicShape)ball.Shape, player?.Shape).Collision)
            {
                ball.PlayerBounce(player);
            }

            if (ball.IsDeleted())
            {
                ballsCount--;
            }

            if (ballsCount <= 0)
            {
                balls?.AddEntity(Ball.AtRandomAngle(0.5f, 0.45f, 0.02f));
                ballsCount++;

                playerLives?.LoseLives();
            }

            blocks?.Container.Iterate(block =>
            {
                if (CollisionDetection.Aabb((DynamicShape)ball.Shape, block.Shape).Collision)
                {
                    ball.Bounce(block);
                }
            });
        });
    }

    /// <summary>
    /// Assesses whether all lives have been lost and loses the game if they have.
    /// </summary>
    private void CheckLives()
    {
        if (playerLives?.GetLives() <= 0)
        {
            int points = -1;
            if (playerRewards != null)
            {
                points = playerRewards.GetPoints();
            }

            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.StatusEvent,
                From = this,
                Message = "Game Over Score",
                IntArg1 = points,
            });
            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.GameStateEvent,
                From = this,
                Message = "GAME_OVER",
            });
        }
    }

    /// <summary>
    /// Updates and moves all active rockets.
    /// </summary>
    private void IterateRockets()
    {
        rockets?.Iterate(rocket =>
        {
            rocket.Shape.Move();

            bool collided = false;

            blocks?.Container.Iterate(block =>
            {
                if (CollisionDetection.Aabb((DynamicShape)rocket.Shape, block.Shape).Collision)
                {
                    collided = true;
                }
            });

            if (collided)
            {
                blocks?.Container.Iterate(block =>
                {
                    DynamicShape explosion = new(rocket.Shape.Position - rocket.explosionExtent / 2,
                                    rocket.explosionExtent);
                    if (Collision.AABB(explosion, block.Shape))
                    {
                        block.Hit();
                    }
                });

                rocket.DeleteEntity();
            }
        });
    }

    /// <summary>
    /// Assesses whether the time has run out in a timed level and loses the game if it has.
    /// </summary>
    private void CheckTimeOut()
    {
        if (time?.GetTime() == 0)
        {
            int points = -1;
            if (playerRewards != null)
            {
                points = playerRewards.GetPoints();
            }

            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.StatusEvent,
                From = this,
                Message = "Game Over Score",
                IntArg1 = points,
            });
            BreakOutBus.GetBus().RegisterEvent(new GameEvent
            {
                EventType = GameEventType.GameStateEvent,
                From = this,
                Message = "GAME_OVER",
            });
        }
    }

    public void KeyPress(KeyboardKey key)
    {
        switch (key)
        {
            case KeyboardKey.Left:

                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.PlayerEvent,
                    From = this,
                    Message = "MoveLeftPress"
                });
                break;
            case KeyboardKey.A:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveLeftPress" });
                break;
            case KeyboardKey.Right:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightPress" });
                break;
            case KeyboardKey.D:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightPress" });
                break;
            case KeyboardKey.Space:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "SpacebarPress" });
                break;
            case KeyboardKey.Escape:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.GameStateEvent, From = this, Message = "GAME_PAUSED" });
                break;
        }
    }

    public void KeyRelease(KeyboardKey key)
    {
        switch (key)
        {
            case KeyboardKey.Left:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.PlayerEvent,
                    From = this,
                    Message = "MoveLeftRelease"
                });
                break;
            case KeyboardKey.A:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveLeftRelease" });
                break;
            case KeyboardKey.Right:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightRelease" });
                break;
            case KeyboardKey.D:
                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                { EventType = GameEventType.PlayerEvent, From = this, Message = "MoveRightRelease" });
                break;
        }
    }

    public void HandleKeyEvent(KeyboardAction keyboardAction, KeyboardKey keyboardKey)
    {
        switch (keyboardAction)
        {
            case (KeyboardAction.KeyPress):
                KeyPress(keyboardKey);
                break;
            case (KeyboardAction.KeyRelease):
                KeyRelease(keyboardKey);
                break;
        }
    }

    public void RenderState()
    {
        if (!isActive)
        {
            return;
        }

        backGroundImage.RenderEntity();
        balls?.RenderEntities();
        rockets?.RenderEntities();
        blocks?.Render();
        player?.Render();
        playerRewards?.RenderPoints();
        effects?.RenderEntities();
        playerLives?.RenderLives();
        time?.RenderLevelTimeOut();
    }
    public void UpdateState()
    {
        if (!isActive)
        {
            return;
        }

        time?.CountDown();
        Time.UpdateDeltaTime();
        player?.Move();

        CheckLevelClear();
        IterateEffects();
        IterateBalls();
        CheckLives();
        IterateRockets();
        CheckTimeOut();

        if (modifierHandler != null)
        {
            Modifiers.Apply(modifierHandler.Flags);
        }
    }

    public void ProcessEvent(GameEvent gameEvent)
    {
        switch (gameEvent.Message)
        {
            case "EffectCreated":
                effects?.AddEntity((Effect)gameEvent.ObjectArg1);
                break;
        }
    }

    /// <summary>
    /// Applies a particular modifier from a collected effect.
    /// </summary>
    public void ApplyModifiers(GameFlag flag)
    {
        if (player == null)
        {
            return;
        }

        if (flag.HasFlag(GameFlag.Rocket))
        {
            if (player.Shoot)
            {
                rockets?.AddEntity(Rocket.Factory(player.Shape.Position.X, player.Shape.Position.Y));

                BreakOutBus.GetBus().RegisterEvent(new GameEvent
                {
                    EventType = GameEventType.StatusEvent,
                    Message = "DisableRocket",
                });
            }
        }
        player.ResetShoot();
    }
}