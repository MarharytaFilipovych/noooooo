using GameMode.ModeConfigurations;

namespace GameMode.Factory;

public interface IGameModeFactory
{
    void RegisterMode(GameModeOption option, IGameModeConfiguration configuration);
    IGameModeConfiguration GetConfiguration(ModeType modeType);
    IGameModeConfiguration GetDefaultConfiguration();  
    IReadOnlyList<GameModeOption> GetAvailableModes();
}

public record GameModeOption(ModeType ModeType, string DisplayName, string Description);