using Bot.Strategy;
using GameMode.ModeConfigurations;

namespace Bot.BotFactory;

public class BotStrategyFactory : IBotStrategyFactory
{
    private readonly Dictionary<BotDifficulty, IBotStrategy> _strategies = new();

    public void RegisterStrategy(BotDifficulty botDifficulty, IBotStrategy botStrategy)
    {
        if (!_strategies.TryAdd(botDifficulty, botStrategy)) 
            throw new ArgumentException($"Duplicate strategy: {botDifficulty}");
    }

    public IBotStrategy GetStrategy(BotDifficulty difficulty) =>
        _strategies.TryGetValue(difficulty, out var strategy)
            ? strategy
            : throw new ArgumentException($"Unknown difficulty: {difficulty}");
}