using Model.Board.Layouts;
using Model.Game.Mode;
using Model.Game.TurnTimer;
using Model.Position;
using Stats;
using Stats.Repository;

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
    public event Action<GameStatistics>? StatsRequested; 
    public event Action<PlayerType.PlayerType>? TurnTimedOut;

    public AtaxxGameWithEvents(IStatisticsRepository statisticsRepository,
        ITurnTimer turnTimer,
        int boardSize = DefaultBoardSize) 
        : base(statisticsRepository, turnTimer, boardSize) { }

    public AtaxxGameWithEvents(IStatisticsRepository statisticsRepository,
        ITurnTimer turnTimer,
        int boardSize, IBoardLayout layout) 
        : base(statisticsRepository, turnTimer, boardSize, layout) { }

    protected override void HandleTimeout()
    {
        if (!IsEnded) TurnTimedOut?.Invoke(CurrentPlayer);
        base.HandleTimeout();
    }

    public override void StartGame()
    {
        base.StartGame();
        GameStarted?.Invoke(GetBoard(), LayoutName);
        TurnChanged?.Invoke(CurrentPlayer);
    }

    public override bool MakeMove(Position.Position from, Position.Position to)
    {
        var move = new Move(from, to);
        var previousPlayer = CurrentPlayer;
        var success = base.MakeMove(from, to);
        
        PublishMoveResult(move, previousPlayer, success);
        return success;
    }

    public bool MakeMove(string fromNotation, string toNotation)
    {
        if (!PositionParser.TryParse(fromNotation, out var from)) return false;
        return PositionParser.TryParse(toNotation, out var to) && MakeMove(from, to);
    }

    private void PublishMoveResult(Move move, PlayerType.PlayerType previousPlayer, bool success)
    {
        if (success)
        {
            MoveMade?.Invoke(move, previousPlayer);
            BoardUpdated?.Invoke(GetBoard());
            
            if (!IsEnded) TurnChanged?.Invoke(CurrentPlayer);
            else EndGame();
        }
        else MoveInvalid?.Invoke(move, previousPlayer);
    }

    public void ShowHint() => HintRequested?.Invoke(GetValidMoves());

    public void EndGame()
    {
        if (Winner == PlayerType.PlayerType.None) GameDrawn?.Invoke();
        else PlayerWon?.Invoke(Winner);
    }

    public void SetMode() => ModeSet?.Invoke(GameMode.Mode);

    public void DisplayStats() => StatsRequested?.Invoke(Statistics);
    
}