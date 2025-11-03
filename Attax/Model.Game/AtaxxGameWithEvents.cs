namespace Model.Game;

using System;
using System.Collections.Generic;
using Model.Core;

public class AtaxxGameWithEvents : AtaxxGame
{
    public event Action<Cell[,], string> GameStarted;
    public event Action<PlayerType> PlayerWon;
    public event Action GameDrawn;
    public event Action<PlayerType> TurnChanged;
    public event Action<Move, PlayerType> MoveMade;
    public event Action<Move, PlayerType> MoveInvalid;
    public event Action<List<Position>> PiecesConverted;
    public event Action<Cell[,]> BoardUpdated;
    public event Action<List<Move>> HintRequested;

    public AtaxxGameWithEvents(int boardSize = 7) : base(boardSize)
    {
    }

    public AtaxxGameWithEvents(int boardSize, Board.IBoardLayout layout) : base(boardSize, layout)
    {
    }

    public void StartGameWithEvents()
    {
        StartGame();
        GameStarted?.Invoke(GetBoard(), LayoutName);
        TurnChanged?.Invoke(CurrentPlayer);
    }

    public bool MakeMoveWithEvents(Position from, Position to)
    {
        var move = new Move(from, to);
        var previousPlayer = CurrentPlayer;
        
        bool success = MakeMove(from, to);
        
        if (success)
        {
            MoveMade?.Invoke(move, previousPlayer);
            BoardUpdated?.Invoke(GetBoard());
            
            if (!IsEnded)
            {
                TurnChanged?.Invoke(CurrentPlayer);
            }
            else
            {
                if (Winner == PlayerType.None)
                {
                    GameDrawn?.Invoke();
                }
                else
                {
                    PlayerWon?.Invoke(Winner);
                }
            }
        }
        else
        {
            MoveInvalid?.Invoke(move, previousPlayer);
        }
        
        return success;
    }

    public bool MakeMoveWithEvents(string fromNotation, string toNotation)
    {
        try
        {
            var from = Position.Parse(fromNotation);
            var to = Position.Parse(toNotation);
            return MakeMoveWithEvents(from, to);
        }
        catch
        {
            return false;
        }
    }

    public void RequestHint()
    {
        var validMoves = GetValidMoves();
        HintRequested?.Invoke(validMoves);
    }
}