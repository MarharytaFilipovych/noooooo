using GameMode;
using GameMode.Factory;
using GameMode.ModeConfigurations;
using Layout.Factory;
using Layout.Layout;
using Model.Game.CareTaker;
using Model.Game.DTOs;
using Model.Game.EndDetector;
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
    private IBoardLayout? _layout;
    private IGameModeConfiguration? _gameMode;

    private readonly ITurnTimer _turnTimer;
    private readonly ICareTaker _careTaker;
    private readonly IMoveValidator _moveValidator;
    private readonly IMoveExecutor _moveExecutor;
    private readonly IMoveGenerator _moveGenerator;
    private readonly IGameEndDetector _endDetector;
    private readonly IStatsTracker _statsTracker;
    private readonly GameProgress _progress;
    private readonly IGameSettings _settings;
    private readonly IBoardLayoutFactory _boardLayoutFactory;
    private readonly IGameModeFactory _gameModeFactory;

    public IGameModeConfiguration GameMode => _gameMode 
        ?? throw new InvalidOperationException("Cannot access game mode before the game has started.");
    
    protected GameStatistics Statistics => _statsTracker.GetStatistics();

    public PlayerType.PlayerType CurrentPlayer => _progress.CurrentPlayer;
    public PlayerType.PlayerType Winner => _progress.Winner;
    public bool IsEnded => _progress.IsEnded;
    protected int TurnNumber => _progress.TurnNumber;

    protected string LayoutName => _layout?.Name
        ?? throw new InvalidOperationException("Cannot get layout before the game has started.");

    protected int BoardSize => _board?.Size
        ?? throw new InvalidOperationException("Cannot get board size before the game has started.");

    public AtaxxGame(
        IStatsTracker statsTracker,
        ITurnTimer turnTimer,
        IMoveValidator validator,
        IMoveExecutor executor,
        IMoveGenerator generator,
        IGameEndDetector endDetector,
        IGameSettings settings,
        ICareTakerFactory careTakerFactory,
        IBoardLayoutFactory boardLayoutFactory,
        IGameModeFactory gameModeFactory)
    {
        _settings = settings;
        _statsTracker = statsTracker;
        _turnTimer = turnTimer;
        _moveValidator = validator;
        _moveExecutor = executor;
        _moveGenerator = generator;
        _boardLayoutFactory = boardLayoutFactory;
        _gameModeFactory = gameModeFactory;
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

        _layout = _settings.LayoutType.HasValue
            ? _boardLayoutFactory.GetLayout(_settings.LayoutType.Value)
            : _boardLayoutFactory.GetRandomLayout();

        _gameMode = _settings.GameModeType.HasValue
            ? _gameModeFactory.GetConfiguration(_settings.GameModeType.Value)
            : _gameModeFactory.GetDefaultConfiguration();

        _board.Initialize(_layout);
        _progress.Initialize();
        _turnTimer.StartTurn();
    }

    public virtual bool MakeMove(Position.Position from, Position.Position to)
    {
        EnsureGameStarted();
        
        if (IsEnded)
            throw new InvalidOperationException("Game has ended.");

        var move = new Move.Move(from, to);

        if (!_moveValidator.IsValidMove(_board, move, CurrentPlayer)) return false;

        if (_gameMode!.ModeType == GameModeType.PvE && !_gameMode.IsBot(CurrentPlayer))
            _careTaker.BackUp();

        ExecuteMove(move);

        if (!IsEnded)
            _turnTimer.ResetTurn();
        else
            _turnTimer.StopTurn();

        return true;
    }

    private void MakeRandomMove()
    {
        if (_board == null) return;

        var randomMove = _moveGenerator.GenerateMove(_board, CurrentPlayer);
        if (randomMove == null) return;
        MakeMove(randomMove.From, randomMove.To);
    }

    public List<Move.Move> GetValidMoves()
    {
        EnsureGameStarted();
        return _moveValidator.GetValidMoves(_board, CurrentPlayer);
    }

    public List<Move.Move> GetValidMoves(PlayerType.PlayerType player)
    {
        EnsureGameStarted();
        return _moveValidator.GetValidMoves(_board, player);
    }

    public Cell GetCell(Position.Position pos)
    {
        EnsureGameStarted();
        return _board.GetCell(pos);
    }

    public Cell[,] GetBoard()
    {
        EnsureGameStarted();
        return _board.GetCells();
    }

    public (int xCount, int oCount) GetPieceCounts()
    {
        EnsureGameStarted();
        return _board.CountPieces();
    }

    public GameState GetGameState()
    {
        EnsureGameStarted();

        var (xCount, oCount) = GetPieceCounts();
        var cells = new CellState[_board.Size, _board.Size];

        for (var row = 0; row < _board.Size; row++)
        {
            for (var col = 0; col < _board.Size; col++)
            {
                var cell = GetCell(new Position.Position(row, col));
                cells[row, col] = new CellState(cell.OccupiedBy, cell.IsBlocked);
            }
        }

        return new GameState(_board.Size, cells, CurrentPlayer, xCount,
            oCount, IsEnded, Winner, LayoutName);
    }

    private void ExecuteMove(Move.Move move)
    {
        _moveExecutor.ExecuteMove(_board!, move, CurrentPlayer);
        SwitchPlayer();
        CheckGameEnd();
    }

    private void SwitchPlayer()
    {
        _progress.AdvanceTurn();

        var opponent = CurrentPlayer.GetOpponent();
        if (_moveValidator.GetValidMoves(_board!, CurrentPlayer).Count == 0 &&
            _moveValidator.GetValidMoves(_board!, opponent).Count > 0)
        {
            _progress.AdvanceTurn();
        }
    }

    private void CheckGameEnd()
    {
        var result = _endDetector.CheckGameEnd(_board!, _moveValidator, CurrentPlayer);

        if (result.IsEnded)
        {
            _progress.EndGame(result.Winner);
            _turnTimer.StopTurn();
            _statsTracker.RecordGame(result.Winner, TurnNumber);
        }
    }

    public bool UndoLastMove()
    {
        EnsureGameStarted();
        return _gameMode!.ModeType == GameModeType.PvE && _careTaker.Undo(UndoTimeWindowSeconds);
    }

    public IMemento Save()
    {
        EnsureGameStarted();
        return new GameMemento(_board.Clone(), CurrentPlayer, TurnNumber);
    }

    public void Restore(IMemento memento)
    {
        if (memento is not GameMemento saved)
            throw new InvalidOperationException("Invalid memento type");

        _board = saved.Board;
        _progress.Restore(saved.CurrentPlayer, saved.TurnNumber);
        _turnTimer.ResetTurn();
    }

    private void EnsureGameStarted()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot perform this operation before the game has started.");
    }

    private class GameMemento(Board.Board board, PlayerType.PlayerType currentPlayer, int turnNumber) : IMemento
    {
        internal Board.Board Board { get; } = board;
        internal PlayerType.PlayerType CurrentPlayer { get; } = currentPlayer;
        internal int TurnNumber { get; } = turnNumber;
    }
}