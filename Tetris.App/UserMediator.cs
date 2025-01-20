using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using TetrisApp;

namespace Tetris.CommonLib;

public class UserMediator : IUserInterface
{
    public UserMediator(MainPage mainPage)
    {
        var handler = mainPage.Handler;
        UIElement? nativeView = handler?.PlatformView as UIElement;
        if (nativeView != null)
        {
            nativeView.KeyDown += NativeViewOnKeyDown;
        }
    }

    private void NativeViewOnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        switch (e.Key)
        {
            case VirtualKey.Left:
                MoveLeft?.Invoke();
                break;
            case VirtualKey.Right:
                MoveRight?.Invoke();
                break;
            case VirtualKey.Down:
                MoveDown?.Invoke();
                break;
            case VirtualKey.Up:
                RotateClockwise?.Invoke();
                break;
        }
    }

    public event Action? MoveLeft;
    public event Action? MoveRight;
    public event Action? MoveDown;
    public event Action? RotateClockwise;
}
