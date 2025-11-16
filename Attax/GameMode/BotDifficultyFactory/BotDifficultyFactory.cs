using GameMode.ModeConfigurations;

namespace GameMode.BotDifficultyFactory;

public class BotDifficultyFactory : IBotDifficultyFactory
{
    private readonly Dictionary<BotDifficulty, BotDifficultyOption> _difficultyOptions = new();

    public void RegisterDifficulty(BotDifficultyOption option)
    {
        ArgumentNullException.ThrowIfNull(option);

        if (_difficultyOptions.ContainsKey(option.Difficulty))
            throw new ArgumentException($"Difficulty {option.Difficulty} is already registered");

        _difficultyOptions[option.Difficulty] = option;
    }

    public IReadOnlyList<BotDifficultyOption> GetAvailableDifficulties() =>
        _difficultyOptions.Values.ToList();

    public BotDifficultyOption GetDifficulty(BotDifficulty difficulty)
    {
        if (!_difficultyOptions.TryGetValue(difficulty, out var option))
            throw new InvalidOperationException($"No difficulty registered for: {difficulty}");

        return option;
    }
}