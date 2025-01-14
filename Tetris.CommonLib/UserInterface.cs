namespace Tetris.CommonLib;

/// <summary>
/// Handles all available user actions
/// </summary>
public interface IUserInterfaceHandler
{
    void OnMoveLeft();

    void OnMoveRight();

    void OnMoveDown();

    void OnRotateClockwise();
}
