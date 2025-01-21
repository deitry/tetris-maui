using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tetris.CommonLib;

namespace TetrisApp;

public class MainPageViewModel : INotifyPropertyChanged
{
    private long _score;
    private bool _isGameOver;

    public long Score
    {
        get => _score;
        set
        {
            if (value == _score) return;
            _score = value;
            OnPropertyChanged();
        }
    }


    public bool IsGameOver
    {
        get => _isGameOver;
        private set
        {
            if (value == _isGameOver) return;
            _isGameOver = value;
            OnPropertyChanged();
        }
    }

    public MainPageViewModel()
    {
        // show message and button to start the game
        IsGameOver = true;

        User = new UserMediator();

        GameController = new GameController(User);

        GameController.ScoreChanged += UpdateScore;
        GameController.GameOver += OnGameOver;
    }

    public UserMediator User { get; }

    public GameController GameController { get; private set; }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    public Task Start()
    {
        IsGameOver = false;

        return GameController.Restart();
    }

    private void OnGameOver()
    {
        IsGameOver = true;
    }

    private void UpdateScore(long newScore)
    {
        Score = newScore;
    }
}
