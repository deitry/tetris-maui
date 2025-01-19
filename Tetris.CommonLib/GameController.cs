using Tetris.CommonLib;

/// <summary>
/// Holds main game cycle and is responsible for score calculation
/// </summary>
public class GameController
{
    private static readonly IReadOnlyList<IShape> AvailableShapes = Shapes.Tetraminoes;

    public event Action<bool[,]>? GameStateUpdated;

    public event Action<PositionedShape>? CurrentShapeUpdated;

    public event Action<long>? ScoreChanged;

    public event Action? GameOver;

    public long Score { get; private set; }

    private int _nextBlockIndex = Random.Shared.Next(0, AvailableShapes.Count);

    public IShape NextBlock => AvailableShapes[_nextBlockIndex];

    /// <summary>
    /// The time between each movement of block.
    /// Should increase over time.
    /// </summary>
    public TimeSpan TickPeriod { get; private set; } = TimeSpan.FromSeconds(2);

    /// <summary>
    /// Each successfull row will
    /// </summary>
    public const double SpeedMultiplier = 0.9999;

    public GameField GameField { get; } = new(width: 10, height: 20);

    public GameController(IUserInterface user)
    {
        // register user action handlers
        user.MoveLeft += GameField.OnMoveLeft;
        user.MoveRight += GameField.OnMoveRight;
        user.MoveDown += GameField.OnMoveDown;
        user.RotateClockwise += GameField.OnRotateClockwise;

        GameField.StateUpdated += GameStateUpdated;

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

        ScoreChanged?.Invoke(Score);
    }

    public async Task GameCycle()
    {
        while (true)
        {
            _nextBlockIndex = Random.Shared.Next(0, AvailableShapes.Count);

            if (GameField.CurrentShape == null)
            {
                if (!GameField.CanSpawn(NextBlock))
                {
                    GameOver?.Invoke();
                    return;
                }

                GameField.Spawn(NextBlock);
            }

            await Task.Delay(TickPeriod);

            GameField.StateBasedActions();
        }
    }
}
