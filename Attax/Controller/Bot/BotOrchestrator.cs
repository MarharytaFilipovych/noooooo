namespace Controller.Bot;

using Model;
using Model.Game;
using System;
using System.Threading;

public class BotOrchestrator
{
    private readonly IBotStrategy _strategy;
    private readonly int _thinkingDelayMs;

    public BotOrchestrator(IBotStrategy strategy, int thinkingDelayMs = 500)
    {
        _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        _thinkingDelayMs = thinkingDelayMs;
    }

    public void MakeBotMove(AtaxxGameWithEvents game, PlayerType botPlayer)
    {
        Thread.Sleep(_thinkingDelayMs);

        var validMoves = game.GetValidMoves(botPlayer);
        
        if (validMoves.Count == 0)
            return;

        var selectedMove = _strategy.SelectMove(validMoves, game, botPlayer);
        game.MakeMoveWithEvents(selectedMove.From, selectedMove.To);
    }
}