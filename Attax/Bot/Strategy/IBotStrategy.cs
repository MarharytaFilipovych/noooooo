using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Strategy;

public interface IBotStrategy
{
    Move.Move SelectMove(List<Move.Move> validMoves, AtaxxGame game, PlayerType botPlayer);
    string Name { get; }
}