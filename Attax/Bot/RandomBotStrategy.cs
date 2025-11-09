using Model;
using Model.Game.Game;
using Model.PlayerType;

namespace Bot;

public class RandomBotStrategy(Random? random = null) : IBotStrategy
{
    private readonly Random _random = random ?? new Random();

    public string Name => "Random Bot";

    public Move SelectMove(List<Move> validMoves, AtaxxGame game, PlayerType botPlayer)
    {
        return validMoves.Count == 0 
            ? throw new InvalidOperationException("No valid moves available")
            : validMoves[_random.Next(validMoves.Count)];
    }
}