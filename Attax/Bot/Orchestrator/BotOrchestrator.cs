using Bot.BotFactory;
using Bot.Strategy;
using GameMode.ModeConfigurations;
using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Orchestrator;

public class BotOrchestrator(IBotStrategyFactory strategyFactory, BotOptions options) : IBotOrchestrator
{
    public Move.Move? SuggestMove(AtaxxGame game, PlayerType botPlayer)
    {
        if (game.GameMode is not PvEConfiguration pveConfiguration)
        {
            throw new InvalidOperationException("BotOrchestrator only supports PvE");
        }

        Thread.Sleep(options.ThinkingDelayMs);

        var validMoves = game.GetValidMoves(botPlayer);

        if (validMoves.Count == 0)
        {
            return null;
        }

        var strategy = strategyFactory.GetStrategy(pveConfiguration.BotDifficulty);
        return strategy.SelectMove(validMoves, game.Board, botPlayer);
    }
}