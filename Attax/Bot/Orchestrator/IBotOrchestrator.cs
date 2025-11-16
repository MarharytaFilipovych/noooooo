using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Orchestrator;

public interface IBotOrchestrator
{
    Move.Move? SuggestMove(AtaxxGame game, PlayerType botPlayer);
}