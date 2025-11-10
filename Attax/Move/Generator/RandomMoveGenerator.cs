using Model.Board;
using Move.Validator;

namespace Move.Generator;

public class RandomMoveGenerator(MoveValidator validator) : IMoveGenerator
{
    private readonly Random _random = new();

    public Move? GenerateMove(Board board, Model.PlayerType.PlayerType player)
    {
        var validMoves = validator.GetValidMoves(board, player);
        return validMoves.Count == 0 ? null : validMoves[_random.Next(validMoves.Count)];
    }
}