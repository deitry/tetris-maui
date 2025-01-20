namespace TetrisApp;

public partial class App : Application
{
    internal static SynchronizationContext UiContext { get; private set; } = null!;

    internal static void RunInUiContext(Action action)
    {
        App.UiContext.Post(_ =>
        {
            action();
        }, null);
    }

    public App()
    {
        InitializeComponent();

        UiContext = SynchronizationContext.Current!;

        MainPage = new AppShell();
    }

}
