namespace Model.Game.Mode;

public class GameModeFactory : IGameModeFactory
{
    private readonly Dictionary<GameMode, Func<GameModeConfiguration>> _modeCreators = new();
    private readonly Dictionary<GameMode, GameModeOption> _modeOptions = new();

    public void RegisterMode(GameModeOption option, Func<GameModeConfiguration> creator)
    {
        ArgumentNullException.ThrowIfNull(option);

        _modeCreators[option.Mode] = creator ?? throw new ArgumentNullException(nameof(creator));
        _modeOptions[option.Mode] = option;
    }

    public GameModeConfiguration CreateMode(GameMode mode) =>
        !_modeCreators.TryGetValue(mode, out var creator) 
            ? throw new InvalidOperationException($"Unknown game mode: {mode}")
            : creator();

    public IReadOnlyList<GameModeOption> GetAvailableModes() => 
        _modeOptions.Values.ToList();
}