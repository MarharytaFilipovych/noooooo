using Model.Board;
using Model.PlayerType;

namespace Bot.Strategy;

public class RandomBotStrategy : IBotStrategy
{
    private readonly Random _random = new();

    public Move.Move SelectMove(List<Move.Move> validMoves, Board board, PlayerType botPlayer) =>
        validMoves.Count == 0
            ? throw new InvalidOperationException("No valid moves available")
            : validMoves[_random.Next(validMoves.Count)];
}