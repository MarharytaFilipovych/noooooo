using Bot.Evaluation;
using Model.Board;
using Model.PlayerType;

namespace Bot.Strategy;

public class GreedyBotStrategy(IMoveEvaluator moveEvaluator) : IBotStrategy
{
    private readonly Random _random = new();

    public Move.Move SelectMove(List<Move.Move> validMoves, Board board, PlayerType botPlayer)
    {
        if (validMoves.Count == 0)
            throw new InvalidOperationException("No valid moves available");

        var bestMoves = new List<Move.Move>();
        var maxScore = int.MinValue;

        foreach (var move in validMoves)
        {
            var score = moveEvaluator.EvaluateMove(move, board, botPlayer);

            if (score > maxScore)
            {
                maxScore = score;
                bestMoves.Clear();
                bestMoves.Add(move);
            }
            else if (score == maxScore) bestMoves.Add(move);
        }

        return bestMoves[_random.Next(bestMoves.Count)];
    }
}