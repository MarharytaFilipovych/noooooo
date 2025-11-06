using Model;

namespace Controller.Bot;

using Model.Game;
using System;
using System.Collections.Generic;

public class RandomBotStrategy : IBotStrategy
{
    private readonly Random _random;

    public string Name => "Random Bot";

    public RandomBotStrategy(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public Move SelectMove(List<Move> validMoves, AtaxxGame game, PlayerType botPlayer)
    {
        if (validMoves.Count == 0)
            throw new InvalidOperationException("No valid moves available");

        return validMoves[_random.Next(validMoves.Count)];
    }
}