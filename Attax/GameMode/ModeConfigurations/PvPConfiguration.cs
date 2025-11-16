using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public sealed class PvPConfiguration : IGameModeConfiguration
{
    public ModeType ModeType => ModeType.PvP;
    public bool IsBot(PlayerType player) => false;
}
