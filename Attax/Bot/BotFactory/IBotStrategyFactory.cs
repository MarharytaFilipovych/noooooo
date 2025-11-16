using Bot.Strategy;
using GameMode.ModeConfigurations;

namespace Bot.BotFactory;

public interface IBotStrategyFactory
{
    void RegisterStrategy(BotDifficulty botDifficulty, IBotStrategy botStrategy);
    IBotStrategy GetStrategy(BotDifficulty difficulty);
}