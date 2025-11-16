using GameMode;
using GameMode.ModeConfigurations;
using Layout;

namespace Model.Game.Settings;

public interface IGameSettings
{
    int BoardSize { get; set; }
    LayoutType? LayoutType { get; set; }
    ModeType? GameModeType { get; set; }
    BotDifficulty? BotDifficulty { get; set; }
    void MarkGameAsStarted();
    void Reset();
}