using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public interface IGameModeConfiguration
{
    public GameModeType ModeType { get; }
    public bool IsBot(PlayerType player);
}
