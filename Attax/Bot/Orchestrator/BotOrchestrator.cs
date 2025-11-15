using Bot.Strategy;
using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Orchestrator;

public class BotOrchestrator(IBotStrategy strategy, BotOptions options) : IBotOrchestrator
{
    private readonly IBotStrategy _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));

    public void MakeBotMove(AtaxxGameWithEvents game, PlayerType botPlayer)
    {
        Thread.Sleep(options.ThinkingDelayMs);

        var validMoves = game.GetValidMoves(botPlayer);
        
        if (validMoves.Count == 0) return;

        var selectedMove = _strategy.SelectMove(validMoves, game, botPlayer);
        game.MakeMove(selectedMove.From, selectedMove.To);
    }
}
