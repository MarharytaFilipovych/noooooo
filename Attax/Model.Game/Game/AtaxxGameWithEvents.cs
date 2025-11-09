using Model.Board.Layouts;
using Model.Game.Mode;
using Model.Position;

namespace Model.Game.Game;

public class AtaxxGameWithEvents : AtaxxGame
{
    public event Action<Cell[,], string>? GameStarted;
    public event Action<PlayerType.PlayerType>? PlayerWon;
    public event Action? GameDrawn;
    public event Action<PlayerType.PlayerType>? TurnChanged;
    public event Action<Move, PlayerType.PlayerType>? MoveMade;
    public event Action<Move, PlayerType.PlayerType>? MoveInvalid;
    public event Action<Cell[,]>? BoardUpdated;
    public event Action<List<Move>>? HintRequested;
    public event Action<GameMode>? ModeSet;

    public AtaxxGameWithEvents(int boardSize = 7) 
        : base(boardSize) { }

    public AtaxxGameWithEvents(int boardSize, IBoardLayout layout) 
        : base(boardSize, layout) { }


    public void StartGame(Cell[,] board, string layoutName, PlayerType.PlayerType currentPlayer)
    {
        StartGame();
        GameStarted?.Invoke(board, layoutName);
        TurnChanged?.Invoke(currentPlayer);
    }

    public bool MakeMoveWithEvents(Position.Position from, Position.Position to)
    {
        var move = new Move(from, to);
        var previousPlayer = CurrentPlayer;
        var success = MakeMove(from, to);
        
        PublishMoveResult(this, move, previousPlayer, success);
        return success;
    }

    public bool MakeMoveWithEvents(string fromNotation, string toNotation)
    {
        if (!PositionParser.TryParse(fromNotation, out var from)) return false;
        return PositionParser.TryParse(toNotation, out var to) && MakeMoveWithEvents(from, to);
    }

    public void PublishMoveResult(AtaxxGame game, Move move, PlayerType.PlayerType previousPlayer, bool success)
    {
        if (success)
        {
            MoveMade?.Invoke(move, previousPlayer);
            BoardUpdated?.Invoke(game.GetBoard());
            
            if (!game.IsEnded) TurnChanged?.Invoke(game.CurrentPlayer);
            else EndGame(game);
        }
        else MoveInvalid?.Invoke(move, previousPlayer);
    }

    public void ShowHint() => HintRequested?.Invoke(GetValidMoves());

    public void EndGame(AtaxxGame game)
    {
        if (game.Winner == PlayerType.PlayerType.None) GameDrawn?.Invoke();
        else PlayerWon?.Invoke(game.Winner);
    }

    public void SetMode() => ModeSet?.Invoke(GameMode.Mode);
}
