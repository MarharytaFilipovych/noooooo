using GameMode;
using Layout.LayoutType;

namespace Model.Game.Settings;

public interface IGameSettings
{
    int BoardSize { get; set; }
    LayoutType? LayoutType { get; set; }
    GameModeType? GameModeType { get; set; }
    void MarkGameAsStarted();
    void Reset();
}