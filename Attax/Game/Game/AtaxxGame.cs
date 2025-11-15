using Layout;
using Layout.Factory;
using Model.Game.CareTaker;
using Model.Game.DTOs;
using Model.Game.EndDetector;
using Model.Game.Mode;
using Model.Game.Settings;
using Model.Game.TurnTimer;
using Model.PlayerType;
using Move.Executor;
using Move.Generator;
using Move.Validator;
using Stats;
using Stats.Tracker;

namespace Model.Game.Game;

public class AtaxxGame
{
    private const int UndoTimeWindowSeconds = 3;
    private Board.Board? _board;
    private Board.Board Board => _board ?? new Board.Board();
    private readonly ITurnTimer _turnTimer;
    private readonly ICareTaker _careTaker;
    private readonly IBoardLayout _layout;
    private readonly IMoveValidator _moveValidator;
    private readonly IMoveExecutor _moveExecutor;
    private readonly IMoveGenerator _moveGenerator;
    private readonly IGameEndDetector _endDetector;
    private readonly IStatsTracker _statsTracker;
    private readonly GameProgress _progress;
    private readonly IGameSettings _settings;

    public GameModeConfiguration GameMode { get; set; } = GameModeConfiguration.CreatePvP();
    protected GameStatistics Statistics => _statsTracker.GetStatistics();
    
    public PlayerType.PlayerType CurrentPlayer => _progress.CurrentPlayer;
    public PlayerType.PlayerType Winner => _progress.Winner;
    public bool IsEnded => _progress.IsEnded;
    protected int TurnNumber => _progress.TurnNumber;
    protected string LayoutName => _layout.Name;

    public AtaxxGame(IStatsTracker statsTracker, ITurnTimer turnTimer, IMoveValidator validator, 
        IMoveExecutor executor, IMoveGenerator generator, IGameEndDetector endDetector,
        IGameSettings settings, ICareTakerFactory careTakerFactory, 
        IBoardLayoutFactory boardLayoutFactory, LayoutType? layoutType = null)
    {
        _settings = settings;
        _statsTracker = statsTracker;
        _turnTimer = turnTimer;
        _moveValidator = validator;
        _moveExecutor = executor;
        _moveGenerator = generator;
        _layout =  layoutType.HasValue ? boardLayoutFactory.GetLayout(layoutType.Value) : boardLayoutFactory.GetRandomLayout();
        _endDetector = endDetector;
        _progress = new GameProgress();
        _turnTimer.TimeoutOccurred += HandleTimeout;
        _careTaker = careTakerFactory.Create(this);
    }
    
    protected virtual void HandleTimeout()
    {
        if (IsEnded) return;
        MakeRandomMove();
    }

    public virtual void StartGame(int? boardSize = null)
    {
        _settings.MarkGameAsStarted();
        var size = boardSize ?? _settings.BoardSize;
        _board = new Board.Board(size);
        _board.Initialize(_layout);
        _progress.Initialize();
        _turnTimer.StartTurn();
    }

    public virtual bool MakeMove(Position.Position from, Position.Position to)
    {
        if (IsEnded) throw new InvalidOperationException("Game has ended");

        var move = new Move.Move(from, to);
        
        if (!_moveValidator.IsValidMove(Board, move, CurrentPlayer))
            return false;

        if (GameMode.Mode == Mode.GameMode.PvE && !GameMode.IsBot(CurrentPlayer)) 
            _careTaker.BackUp();

        ExecuteMove(move);
        
        if (!IsEnded) _turnTimer.ResetTurn();
        else _turnTimer.StopTurn();
        
        return true;
    }

    private void MakeRandomMove()
    {
        var randomMove = _moveGenerator.GenerateMove(Board, CurrentPlayer);
        if (randomMove == null) return;
        MakeMove(randomMove.From, randomMove.To);
    }

    public List<Move.Move> GetValidMoves() => _moveValidator.GetValidMoves(Board, CurrentPlayer);

    public List<Move.Move> GetValidMoves(PlayerType.PlayerType player) => 
        _moveValidator.GetValidMoves(Board, player);

    public Cell GetCell(Position.Position pos) => Board.GetCell(pos);

    public Cell[,] GetBoard() => Board.GetCells();

    public (int xCount, int oCount) GetPieceCounts() => Board.CountPieces();
    
    public GameState GetGameState()
    {
        var (xCount, oCount) = GetPieceCounts();
        var cells = new CellState[Board.Size, Board.Size];
    
        for (var row = 0; row < Board.Size; row++)
        {
            for (var col = 0; col < Board.Size; col++)
            {
                var cell = GetCell(new Position.Position(row, col));
                cells[row, col] = new CellState(cell.OccupiedBy, cell.IsBlocked);
            }
        }

        return new GameState(Board.Size, cells, CurrentPlayer, xCount,
            oCount, IsEnded, Winner, LayoutName);
    }

    private void ExecuteMove(Move.Move move)
    {
        _moveExecutor.ExecuteMove(Board, move, CurrentPlayer);
        SwitchPlayer();
        CheckGameEnd();
    }

    private void SwitchPlayer()
    {
        _progress.AdvanceTurn();
        
        var opponent = CurrentPlayer.GetOpponent();
        if (_moveValidator.GetValidMoves(Board, CurrentPlayer).Count == 0 && 
            _moveValidator.GetValidMoves(Board, opponent).Count > 0) 
            _progress.AdvanceTurn();
    }

    private void CheckGameEnd()
    {
        var result = _endDetector.CheckGameEnd(Board, _moveValidator, CurrentPlayer);
        
        if (result.IsEnded)
        {
            _progress.EndGame(result.Winner);
            _turnTimer.StopTurn();
            _statsTracker.RecordGame(result.Winner, TurnNumber);
        }
    }
    
    public bool UndoLastMove() =>
        GameMode.Mode == Mode.GameMode.PvE && _careTaker.Undo(UndoTimeWindowSeconds);
    
    public IMemento Save() => 
        new GameMemento(Board.Clone(), CurrentPlayer, TurnNumber);

    public void Restore(IMemento memento)
    {
        if (memento is not GameMemento saved) 
            throw new InvalidOperationException("Invalid memento type");

        _board = saved.Board;
        _progress.Restore(saved.CurrentPlayer, saved.TurnNumber);
        _turnTimer.ResetTurn();
    }
    
    private class GameMemento(
        Board.Board board, 
        PlayerType.PlayerType currentPlayer,
        int turnNumber) : IMemento
    {
        internal Board.Board Board { get; } = board;
        internal PlayerType.PlayerType CurrentPlayer { get; } = currentPlayer;
        internal int TurnNumber { get; } = turnNumber;
    }
}