using JetBrains.Annotations;

namespace Tetris.CommonLib;

/// <summary>
/// Handles all available user actions
/// </summary>
[PublicAPI]
public interface IUserInterfaceHandler
{
    void OnMoveLeft();

    void OnMoveRight();

    void OnMoveDown();

    void OnRotateClockwise();
}
