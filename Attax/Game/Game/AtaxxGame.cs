using Layout.Factory;
using Layout.Layout;
using Layout.LayoutType;
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
    private IBoardLayout? _layout;

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

    public GameModeConfiguration GameMode { get; set; } = GameModeConfiguration.CreatePvP();
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
        IBoardLayoutFactory boardLayoutFactory)
    {
        _settings = settings;
        _statsTracker = statsTracker;
        _turnTimer = turnTimer;
        _moveValidator = validator;
        _moveExecutor = executor;
        _moveGenerator = generator;
        _boardLayoutFactory = boardLayoutFactory;
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

        _board.Initialize(_layout);
        _progress.Initialize();
        _turnTimer.StartTurn();
    }

    public virtual bool MakeMove(Position.Position from, Position.Position to)
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot make move before the game has started.");
        if (IsEnded)
            throw new InvalidOperationException("Game has ended.");

        var move = new Move.Move(from, to);

        if (!_moveValidator.IsValidMove(_board, move, CurrentPlayer)) return false;

        if (GameMode.Mode == Mode.GameMode.PvE && !GameMode.IsBot(CurrentPlayer))
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
        if (_board == null)
            throw new InvalidOperationException("Cannot get valid moves before the game has started.");
        return _moveValidator.GetValidMoves(_board, CurrentPlayer);
    }

    public List<Move.Move> GetValidMoves(PlayerType.PlayerType player)
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot get valid moves before the game has started.");
        return _moveValidator.GetValidMoves(_board, player);
    }

    public Cell GetCell(Position.Position pos)
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot access cells before the game has started.");
        return _board.GetCell(pos);
    }

    public Cell[,] GetBoard()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot access board before the game has started.");
        return _board.GetCells();
    }

    public (int xCount, int oCount) GetPieceCounts()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot count pieces before the game has started.");
        return _board.CountPieces();
    }

    public GameState GetGameState()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot get game state before the game has started.");

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
        if (_board == null) return;

        _moveExecutor.ExecuteMove(_board, move, CurrentPlayer);
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
        if (_board == null) return;

        var result = _endDetector.CheckGameEnd(_board, _moveValidator, CurrentPlayer);

        if (result.IsEnded)
        {
            _progress.EndGame(result.Winner);
            _turnTimer.StopTurn();
            _statsTracker.RecordGame(result.Winner, TurnNumber);
        }
    }

    public bool UndoLastMove()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot undo move before the game has started.");

        return GameMode.Mode == Mode.GameMode.PvE && _careTaker.Undo(UndoTimeWindowSeconds);
    }

    public IMemento Save()
    {
        if (_board == null)
            throw new InvalidOperationException("Cannot save before the game has started.");

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

    private class GameMemento(Board.Board board, PlayerType.PlayerType currentPlayer, int turnNumber) : IMemento
    {
        internal Board.Board Board { get; } = board;
        internal PlayerType.PlayerType CurrentPlayer { get; } = currentPlayer;
        internal int TurnNumber { get; } = turnNumber;
    }
}
