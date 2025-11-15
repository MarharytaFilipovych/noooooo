using GameMode.ModeConfigurations;
using GameMode.ModeType;

namespace GameMode.Factory;

public class GameModeFactory : IGameModeFactory
{
    private readonly Dictionary<GameModeType, IGameModeConfiguration> _configurations = new();
    private readonly Dictionary<GameModeType, GameModeOption> _modeOptions = new();
    private IGameModeConfiguration? _defaultConfiguration;

    public void RegisterMode(GameModeOption option, IGameModeConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(option);
        ArgumentNullException.ThrowIfNull(configuration);

        if (option.ModeType != configuration.ModeType)
            throw new ArgumentException(
                $"Mode mismatch: option mode is {option.ModeType} but configuration mode is {configuration.ModeType}");

        _configurations[option.ModeType] = configuration;
        _modeOptions[option.ModeType] = option;

        _defaultConfiguration ??= configuration;
    }
    
    public IGameModeConfiguration GetDefaultConfiguration() => 
        _defaultConfiguration ?? throw new InvalidOperationException("No game modes have been registered.");

    public IGameModeConfiguration GetConfiguration(GameModeType modeType) => 
        !_configurations.TryGetValue(modeType, out var config) 
            ? throw new InvalidOperationException($"No configuration registered for modeType: {modeType}")
            : config;
    

    public IReadOnlyList<GameModeOption> GetAvailableModes() => 
        _modeOptions.Values.ToList();
}