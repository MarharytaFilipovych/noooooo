using Model.Game.Game;
using Model.PlayerType;

namespace Bot.Strategy;

public class RandomBotStrategy : IBotStrategy
{
    private readonly Random _random = new();
    
    public string Name => "Random Bot";

    public Move.Move SelectMove(List<Move.Move> validMoves, AtaxxGame game, PlayerType botPlayer)
    {
        return validMoves.Count == 0 
            ? throw new InvalidOperationException("No valid moves available")
            : validMoves[_random.Next(validMoves.Count)];
    }
}