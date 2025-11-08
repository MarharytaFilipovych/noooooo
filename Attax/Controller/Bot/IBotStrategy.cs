using Model;
using Model.Game.Game;
using Model.PlayerType;

namespace Attax.Bot;

public interface IBotStrategy
{
    Move SelectMove(List<Move> validMoves, AtaxxGame game, PlayerType botPlayer);
    string Name { get; }
}