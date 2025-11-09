using Model.Game.Game;
using Model.PlayerType;

namespace Attax.Bot;

public class BotOrchestrator(IBotStrategy strategy, int thinkingDelayMs = 500)
{
    private readonly IBotStrategy _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));

    public void MakeBotMove(AtaxxGameWithEvents game, PlayerType botPlayer)
    {
        Thread.Sleep(thinkingDelayMs);

        var validMoves = game.GetValidMoves(botPlayer);
        
        if (validMoves.Count == 0) return;

        var selectedMove = _strategy.SelectMove(validMoves, game, botPlayer);
        game.MakeMoveWithEvents(selectedMove.From, selectedMove.To);
    }
}