namespace Model.Game.Mode;

public class GameModeFactory : IGameModeFactory
{
    private readonly Dictionary<string, Func<GameModeConfiguration>> _modeCreators = new();
    private readonly Dictionary<string, GameModeOption> _modeOptions = new();

    public void RegisterMode(GameModeOption option, Func<GameModeConfiguration> creator)
    {
        _modeCreators[option.Key] = creator ?? throw new ArgumentNullException(nameof(creator));
        _modeOptions[option.Key] = option;
    }

    public GameModeConfiguration RegisterMode(string modeKey) =>
        !_modeCreators.TryGetValue(modeKey, out var creator) 
            ? throw new InvalidOperationException($"Unknown game mode: {modeKey}")
            : creator();

    public IReadOnlyList<GameModeOption> GetAvailableModes() => 
        _modeOptions.Values.ToList();
}