using Model.Board;
using Model.PlayerType;

namespace Bot.Evaluation;

public interface IMoveEvaluator
{
    int EvaluateMove(Move.Move move, Board board, PlayerType player);
}