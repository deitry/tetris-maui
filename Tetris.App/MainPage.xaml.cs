using Tetris.CommonLib;

namespace TetrisApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var user = new UserMediator(this);
        var gameController = new GameController(user);

        TetrisPresenter.SetGameController(gameController);

        Task.Run(gameController.GameCycle);
    }
}
