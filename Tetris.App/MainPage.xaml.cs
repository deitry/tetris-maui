using Tetris.CommonLib;

namespace TetrisApp;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel ViewModel;

    public MainPage()
    {
        ViewModel = new MainPageViewModel();
        BindingContext = ViewModel;

        InitializeComponent();

        TetrisPresenter.SetGameController(ViewModel.GameController);
    }

    private void StartButton_OnClicked(object? sender, EventArgs e)
    {
        Task.Run(ViewModel.Start);
    }
}
