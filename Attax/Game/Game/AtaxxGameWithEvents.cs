using Layout.Factory;
using Model.Game.CareTaker;
using Model.Game.EndDetector;
using Model.Game.Mode;
using Model.Game.Settings;
using Model.Game.TurnTimer;
using Model.Position;
using Stats;
using Stats.Tracker;
using Move.Executor;
using Move.Generator;
using Move.Validator;

namespace Model.Game.Game;

public class AtaxxGameWithEvents(IStatsTracker statsTracker, ITurnTimer turnTimer,
    IMoveValidator validator, IMoveExecutor executor,
    IMoveGenerator generator, IGameEndDetector endDetector,
    IGameSettings settings, ICareTakerFactory careTakerFactory, 
    IBoardLayoutFactory boardLayoutFactory)
    : AtaxxGame(statsTracker, turnTimer, validator, executor, 
        generator, endDetector, settings, careTakerFactory, boardLayoutFactory)
{
    public event Action<Cell[,], string>? GameStarted;
    public event Action<PlayerType.PlayerType>? PlayerWon;
    public event Action? GameDrawn;
    public event Action<PlayerType.PlayerType>? TurnChanged;
    public event Action<Move.Move, PlayerType.PlayerType>? MoveMade;
    public event Action<Move.Move, PlayerType.PlayerType>? MoveInvalid;
    public event Action<Cell[,]>? BoardUpdated;
    public event Action<List<Move.Move>>? HintRequested;
    public event Action<GameMode>? ModeSet;
    public event Action<GameStatistics>? StatsRequested; 
    public event Action<PlayerType.PlayerType>? TurnTimedOut;
    public event Action<bool, PlayerType.PlayerType>? MoveUndone;
    public event Action<List<(string Name, string Usage, string Description)>>? HelpRequested;
    public event Action<string>? ErrorOccurred;

    private void RaiseError(string message) => ErrorOccurred?.Invoke(message);

    public void RequestHelp(List<(string Name, string Usage, string Description)> availableCommands) 
        => HelpRequested?.Invoke(availableCommands);

    protected override void HandleTimeout()
    {
        if (!IsEnded) TurnTimedOut?.Invoke(CurrentPlayer);
        base.HandleTimeout();
    }

    public override void StartGame(int? boardSize = null)
    {
        base.StartGame(boardSize);
        GameStarted?.Invoke(GetBoard(), LayoutName);
        TurnChanged?.Invoke(CurrentPlayer);
    }

    public override bool MakeMove(Position.Position from, Position.Position to)
    {
        var move = new Move.Move(from, to);
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

    private void PublishMoveResult(Move.Move move, PlayerType.PlayerType previousPlayer, bool success)
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

    public new void UndoLastMove()
    {
        if (GameMode.Mode !=  Mode.GameMode.PvE)
        {
            RaiseError("Undo is only available in Player vs Bot mode");
            return;
        }
        
        var success = base.UndoLastMove();
        MoveUndone?.Invoke(success, CurrentPlayer);
        
        
        if (success)
        {
            BoardUpdated?.Invoke(GetBoard());
            TurnChanged?.Invoke(CurrentPlayer);
        }
    }
}