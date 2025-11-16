using Model.Board;
using Model.PlayerType;
using Model.Position;
using Move.Executor;
using Move.Validator;

namespace Bot.Evaluation;

public class GreedyMoveEvaluator(
    IMoveExecutor moveExecutor,
    IMoveValidator moveValidator,
    EvaluationWeights? weights = null) : IMoveEvaluator
{
    private readonly EvaluationWeights _weights = weights ?? EvaluationWeights.Default;

    public int EvaluateMove(Move.Move move, Board board, PlayerType playerType)
    {
        var clonedBoard = board.Clone();
        moveExecutor.ExecuteMove(clonedBoard, move, playerType);

        return CalculateScore(clonedBoard, playerType);
    }

    private int CalculateScore(Board board, PlayerType playerType)
    {
        var opponent = playerType.GetOpponent();

        var (xCount, oCount) = board.CountPieces();

        var myPieces = playerType == PlayerType.X ? xCount : oCount;
        var oppPieces = playerType == PlayerType.X ? oCount : xCount;
        var pieceDifference = myPieces - oppPieces;
        var movesDifference = CountValidMoves(board, playerType) - CountValidMoves(board, opponent);

        return pieceDifference * _weights.PieceDifferenceWeight + movesDifference * _weights.MovesWeight;
    }

    private int CountValidMoves(Board board, PlayerType playerType)
    {
        return moveValidator.GetValidMoves(board, playerType).Count;
    }
}