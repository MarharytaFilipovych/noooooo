using Bot.Strategy;
using GameMode.ModeConfigurations;

namespace Bot.BotFactory;

public class BotStrategyFactory : IBotStrategyFactory
{
    private readonly Dictionary<BotDifficulty, IBotStrategy> _strategies = new();

    public void RegisterStrategy(BotDifficulty botDifficulty, IBotStrategy botStrategy)
    {
        if (_strategies.ContainsKey(botDifficulty))
        {
            throw new ArgumentException($"Duplicate strategy: {botDifficulty}");
        }

        _strategies[botDifficulty] = botStrategy;
    }

    public IBotStrategy GetStrategy(BotDifficulty difficulty)
    {
        return _strategies.TryGetValue(difficulty, out var strategy)
            ? strategy
            : throw new ArgumentException($"Unknown difficulty: {difficulty}");
    }
    
}