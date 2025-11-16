using Bot.Strategy;
using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Orchestrator;

public class BotOrchestrator(IBotStrategy strategy, BotOptions options) : IBotOrchestrator
{
    public Move.Move? SuggestMove(AtaxxGame game, PlayerType botPlayer)
    {
        Thread.Sleep(options.ThinkingDelayMs);

        var validMoves = game.GetValidMoves(botPlayer);

        if (validMoves.Count == 0)
        {
            return null;
        }

        return strategy.SelectMove(validMoves, game.Board, botPlayer);
    }
}
