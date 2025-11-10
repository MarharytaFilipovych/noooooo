using Model.Board;
using Model.PlayerType;

namespace Move.Generator;

public interface IMoveGenerator
{
    Move? GenerateMove(Board board, PlayerType player);
}