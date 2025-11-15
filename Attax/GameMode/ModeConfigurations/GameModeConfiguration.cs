using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public interface IGameModeConfiguration
{
    public ModeType.GameModeType ModeType { get; }
    public bool IsBot(PlayerType player);
}
