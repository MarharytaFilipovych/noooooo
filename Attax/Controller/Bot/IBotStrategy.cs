using Model;

namespace Controller.Bot;

using Model.Game;
using System.Collections.Generic;

public interface IBotStrategy
{
    Move SelectMove(List<Move> validMoves, AtaxxGame game, PlayerType botPlayer);
    string Name { get; }
}