using Model.Game.Game;

namespace Model.Game.EventPublisher;


using System;
using System.Collections.Generic;

public interface IGameEventPublisher
{
    event Action<Cell[,], string>? GameStarted;
    event Action<PlayerType.PlayerType>? PlayerWon;
    event Action? GameDrawn;
    event Action<PlayerType.PlayerType>? TurnChanged;
    event Action<Move, PlayerType.PlayerType>? MoveMade;
    event Action<Move, PlayerType.PlayerType>? MoveInvalid;
    event Action<Cell[,]>? BoardUpdated;
    event Action<List<Move>>? HintRequested;

    void PublishGameStart(Cell[,] board, string layoutName, PlayerType.PlayerType currentPlayer);
    void PublishMoveResult(AtaxxGame game, Move move, PlayerType.PlayerType previousPlayer, bool success);
    void PublishHint(List<Move> validMoves);
}
