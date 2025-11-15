using GameMode.ModeType;
using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public sealed class PvPConfiguration : IGameModeConfiguration
{
    public GameModeType ModeType => GameModeType.PvP;
    public bool IsBot(PlayerType player) => false;
}
