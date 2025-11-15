using GameMode.ModeConfigurations;
using GameMode.ModeType;

namespace GameMode.Factory;

public interface IGameModeFactory
{
    void RegisterMode(GameModeOption option, IGameModeConfiguration configuration);
    IGameModeConfiguration GetConfiguration(GameModeType modeType);
    IGameModeConfiguration GetDefaultConfiguration();  
    IReadOnlyList<GameModeOption> GetAvailableModes();
}

public record GameModeOption(GameModeType ModeType, string DisplayName, string Description);