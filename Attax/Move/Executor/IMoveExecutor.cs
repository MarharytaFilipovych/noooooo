using Model.Board;
using Model.PlayerType;

namespace Move.Executor;

public interface IMoveExecutor
{
    List<Model.Position.Position> ExecuteMove(Board board, Move move, PlayerType player);
}