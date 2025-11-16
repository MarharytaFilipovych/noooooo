using Model.Board;
using Model.PlayerType;

namespace Bot.Strategy;

public interface IBotStrategy
{
    Move.Move SelectMove(List<Move.Move> validMoves, Board board, PlayerType botPlayer);
}