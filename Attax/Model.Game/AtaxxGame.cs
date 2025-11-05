namespace Model.Game;

using System;
using System.Collections.Generic;
using Model.Core;
using Model.Board;

public class AtaxxGame
{
    private readonly Board board;
    private readonly IBoardLayout layout;
    
    public PlayerType CurrentPlayer { get; private set; }
    public PlayerType Winner { get; private set; }
    public bool IsEnded { get; private set; }
    public int TurnNumber { get; private set; }
    
    public int BoardSize => board.Size;
    public string LayoutName => layout.Name;

    public AtaxxGame(int boardSize = 7)
    {
        board = new Board(boardSize);
        layout = BoardLayoutFactory.GetRandomLayout();
    }

    public AtaxxGame(int boardSize, IBoardLayout boardLayout)
    {
        board = new Board(boardSize);
        layout = boardLayout;
    }

    private AtaxxGame(Board clonedBoard, IBoardLayout boardLayout)
    {
        board = clonedBoard;
        layout = boardLayout;
    }

    public void StartGame()
    {
        board.Initialize(layout);
        CurrentPlayer = PlayerType.X;
        Winner = PlayerType.None;
        IsEnded = false;
        TurnNumber = 1;
    }

    public bool MakeMove(Position from, Position to)
    {
        if (IsEnded)
        {
            throw new InvalidOperationException("Game has ended");
        }

        var move = new Move(from, to);
        
        if (!board.IsValidMove(move, CurrentPlayer))
        {
            return false;
        }

        ExecuteMove(move);
        return true;
    }

    public bool MakeMove(string fromNotation, string toNotation)
    {
        try
        {
            var from = Position.Parse(fromNotation);
            var to = Position.Parse(toNotation);
            return MakeMove(from, to);
        }
        catch
        {
            return false;
        }
    }

    public List<Move> GetValidMoves()
    {
        return board.GetValidMoves(CurrentPlayer);
    }

    public List<Move> GetValidMoves(PlayerType player)
    {
        return board.GetValidMoves(player);
    }

    public Cell GetCell(Position pos)
    {
        return board.GetCell(pos);
    }

    public Cell[,] GetBoard()
    {
        return board.GetCells();
    }

    public (int xCount, int oCount) GetPieceCounts()
    {
        return board.CountPieces();
    }

    public bool HasValidMoves()
    {
        return GetValidMoves().Count > 0;
    }

    public bool HasValidMoves(PlayerType player)
    {
        return board.GetValidMoves(player).Count > 0;
    }

    private void ExecuteMove(Move move)
    {
        board.ExecuteMove(move, CurrentPlayer);
        SwitchPlayer();
        CheckGameEnd();
    }

    private void SwitchPlayer()
    {
        var nextPlayer = GetOpponent(CurrentPlayer);
        CurrentPlayer = nextPlayer;
        TurnNumber++;
        
        if (!HasValidMoves(CurrentPlayer) && HasValidMoves(GetOpponent(CurrentPlayer)))
        {
            CurrentPlayer = GetOpponent(CurrentPlayer);
        }
    }

    private void CheckGameEnd()
    {
        var (xCount, oCount) = board.CountPieces();

        if (xCount == 0)
        {
            EndGame(PlayerType.O);
            return;
        }
        if (oCount == 0)
        {
            EndGame(PlayerType.X);
            return;
        }

        if (board.IsFull())
        {
            if (xCount > oCount)
                EndGame(PlayerType.X);
            else if (oCount > xCount)
                EndGame(PlayerType.O);
            else
                EndGame(PlayerType.None);
            return;
        }

        bool currentCanMove = HasValidMoves(CurrentPlayer);
        bool opponentCanMove = HasValidMoves(GetOpponent(CurrentPlayer));

        if (!currentCanMove && !opponentCanMove)
        {
            if (xCount > oCount)
                EndGame(PlayerType.X);
            else if (oCount > xCount)
                EndGame(PlayerType.O);
            else
                EndGame(PlayerType.None);
        }
    }

    private void EndGame(PlayerType winner)
    {
        IsEnded = true;
        Winner = winner;
    }

    private PlayerType GetOpponent(PlayerType player)
    {
        return player == PlayerType.X ? PlayerType.O : PlayerType.X;
    }

    public AtaxxGame Clone()
    {
        var clonedGame = new AtaxxGame(board.Clone(), layout);
        clonedGame.CurrentPlayer = this.CurrentPlayer;
        clonedGame.TurnNumber = this.TurnNumber;
        clonedGame.IsEnded = this.IsEnded;
        clonedGame.Winner = this.Winner;
        
        return clonedGame;
    }
}
