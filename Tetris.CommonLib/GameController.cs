using JetBrains.Annotations;
using Tetris.CommonLib;

/// <summary>
/// Holds main game cycle and is responsible for score calculation
/// </summary>
[PublicAPI]
public class GameController
{
    private static readonly IReadOnlyList<IShape> AvailableShapes = Shapes.Tetraminoes;

    public event Action<bool[,]>? GameStateUpdated;

    public event Action<PositionedShape>? CurrentShapeUpdated;

    public event Action<long>? ScoreChanged;

    public event Action? GameOver;

    public long Score
    {
        get => _score;
        private set
        {
            _score = value;
            ScoreChanged?.Invoke(value);
        }
    }

    private int _nextBlockIndex = Random.Shared.Next(0, AvailableShapes.Count);
    private long _score;

    public IShape NextBlock => AvailableShapes[_nextBlockIndex];

    /// <summary>
    /// The time between each movement of block.
    /// Should increase over time.
    /// </summary>
    public TimeSpan TickPeriod { get; private set; } = TimeSpan.FromSeconds(0.3);

    /// <summary>
    /// Each successfull row will
    /// </summary>
    public const double SpeedMultiplier = 0.9999;

    public GameField GameField { get; } = new(width: 10, height: 20);

    public GameController()
    {
        // dummy
    }

    public GameController(IUserInterface user)
    {
        // register user action handlers
        user.MoveLeft += GameField.OnMoveLeft;
        user.MoveRight += GameField.OnMoveRight;
        user.MoveDown += GameField.OnMoveDown;
        user.RotateClockwise += GameField.OnRotateClockwise;

        GameField.StateUpdated += state => GameStateUpdated?.Invoke(state);
        GameField.CurrentShapeUpdated += shape => CurrentShapeUpdated?.Invoke(shape);

        GameField.RowsCleared += OnRowsCleared;
    }

    private void OnRowsCleared(int rowsCleared)
    {
        var reward = rowsCleared switch
        {
            1 => 100,
            2 => 300,
            3 => 800,
            >= 4 => 2000,
            _ => 0,
        };

        if (reward > 0)
        {
            Score += reward;
            TickPeriod *= SpeedMultiplier;
        }
    }

    public async Task GameCycle()
    {
        try
        {
            while (true)
            {
                if (GameField.CurrentShape == null)
                {
                    _nextBlockIndex = Random.Shared.Next(0, AvailableShapes.Count);
                    GameField.Spawn(NextBlock);
                }

                await Task.Delay(TickPeriod);

                GameField.StateBasedActions();
            }
        }
        catch (GameOverException)
        {
            GameOver?.Invoke();
        }
    }

    public Task Restart()
    {
        Score = 0;
        GameField.Clear();

        return GameCycle();
    }
}
