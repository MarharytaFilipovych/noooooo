using Model.Game.Game;
using Model.PlayerType;

namespace Bot;

public interface IBotOrchestrator
{
    void MakeBotMove(AtaxxGameWithEvents game, PlayerType botPlayer);
}