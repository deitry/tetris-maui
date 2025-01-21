using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using SharpHook;
using SharpHook.Native;
using TetrisApp;

namespace Tetris.CommonLib;

public class UserMediator : IUserInterface
{
    public UserMediator()
    {
        var hook = new TaskPoolGlobalHook();
        hook.KeyPressed += HookOnKeyPressed;

        Task.Run(hook.Run);
        // var handler = element.Handler;
        // UIElement? nativeView = handler?.PlatformView as UIElement;
        // if (nativeView != null)
        // {
        //     nativeView.KeyDown += NativeViewOnKeyDown;
        // }
    }

    private void HookOnKeyPressed(object? sender, KeyboardHookEventArgs e)
    {
        switch (e.Data.KeyCode)
        {
            case KeyCode.VcLeft or KeyCode.VcA:
                MoveLeft?.Invoke();
                break;
            case KeyCode.VcRight or KeyCode.VcD:
                MoveRight?.Invoke();
                break;
            case KeyCode.VcDown or KeyCode.VcS or KeyCode.VcSpace:
                MoveDown?.Invoke();
                break;
            case KeyCode.VcUp or KeyCode.VcW:
                RotateClockwise?.Invoke();
                break;
        }
    }

    public event Action? MoveLeft;
    public event Action? MoveRight;
    public event Action? MoveDown;
    public event Action? RotateClockwise;
}
