using Tetris.CommonLib;

/// <summary>
/// Holds main game cycle and is responsible for score calculation
/// </summary>
public class GameController
{
    private static readonly IReadOnlyList<IShape> AvailableShapes = Shapes.Tetraminoes;

    public event Action<long>? GameOver;

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
    public double SpeedMultiplier { get; set; } = 0.9999;

    public GameField GameField { get; set; } = new();

    public GameController(IUserInterface user)
    {
        // register user action handlers
        user.MoveLeft += GameField.OnMoveLeft;
        user.MoveRight += GameField.OnMoveRight;
        user.MoveDown += GameField.OnMoveDown;
        user.RotateClockwise += GameField.OnRotateClockwise;
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
                    GameOver?.Invoke(Score);
                    return;
                }

                GameField.Spawn(NextBlock);
            }

            await Task.Delay(TickPeriod);

            var result = GameField.StateBasedActions();

            var reward = result.RowsCleared switch
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
    }
}

public interface IUserInterface
{
    event Action MoveLeft;
    event Action MoveRight;
    event Action MoveDown;
    event Action RotateClockwise;
}
