using GameMode;
using GameMode.ModeConfigurations;
using GameMode.ModeType;
using Layout.LayoutType;

namespace Model.Game.Settings;

public interface IGameSettings
{
    int BoardSize { get; set; }
    LayoutType? LayoutType { get; set; }
    GameModeType? GameModeType { get; set; }
    BotDifficulty? BotDifficulty { get; set; }
    void MarkGameAsStarted();
    void Reset();
}