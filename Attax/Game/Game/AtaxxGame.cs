using Layout;
using Model.Game.CareTaker;
using Model.Game.DTOs;
using Model.Game.EndDetector;
using Model.Game.Mode;
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
    protected const int DefaultBoardSize = 7;
    
    private Board.Board _board;
    private readonly ITurnTimer _turnTimer;
    private readonly ICareTaker _careTaker;
    private readonly IBoardLayout _layout;
    private readonly IMoveValidator _moveValidator;
    private readonly IMoveExecutor _moveExecutor;
    private readonly IMoveGenerator _moveGenerator;
    private readonly IGameEndDetector _endDetector;
    private readonly IStatsTracker _statsTracker;
    private readonly GameProgress _progress;
    
    public GameModeConfiguration GameMode { get; set; } = GameModeConfiguration.CreatePvP();
    protected GameStatistics Statistics => _statsTracker.GetStatistics();
    
    public PlayerType.PlayerType CurrentPlayer => _progress.CurrentPlayer;
    public PlayerType.PlayerType Winner => _progress.Winner;
    public bool IsEnded => _progress.IsEnded;
    public int TurnNumber => _progress.TurnNumber;
    public int BoardSize => _board.Size;
    public string LayoutName => _layout.Name;

    public AtaxxGame(IStatsTracker statsTracker, ITurnTimer turnTimer, IMoveValidator validator, 
        IMoveExecutor executor, IMoveGenerator generator, IGameEndDetector endDetector, int boardSize = DefaultBoardSize, IBoardLayout? layout = null)
    {
        _statsTracker = statsTracker;
        _turnTimer = turnTimer;
        _moveValidator = validator;
        _moveExecutor = executor;
        _moveGenerator = generator;
        _board = new Board.Board(boardSize);
        _layout = layout ?? BoardLayoutFactory.GetRandomLayout();
        _careTaker = new CareTaker.CareTaker(this);
        _endDetector = endDetector;
        _progress = new GameProgress();
        _turnTimer.TimeoutOccurred += HandleTimeout;
    }
    

    private AtaxxGame(IStatsTracker statsTracker, ITurnTimer turnTimer, IMoveValidator validator, 
        IMoveExecutor executor, IMoveGenerator generator, IGameEndDetector endDetector, Board.Board clonedBoard, IBoardLayout layout)
    {
        _statsTracker = statsTracker;
        _turnTimer = turnTimer;
        _moveValidator = validator;
        _moveExecutor = executor;
        _moveGenerator = generator;
        _board = clonedBoard;
        _layout = layout;
        _careTaker = new CareTaker.CareTaker(this);
        _endDetector = endDetector;
        _progress = new GameProgress();
        _turnTimer.TimeoutOccurred += HandleTimeout;
    }
    
    protected virtual void HandleTimeout()
    {
        if (IsEnded) return;
        MakeRandomMove();
    }

    public virtual void StartGame()
    {
        _board.Initialize(_layout);
        _progress.Initialize();
        _turnTimer.StartTurn();
    }

    public virtual bool MakeMove(Position.Position from, Position.Position to)
    {
        if (IsEnded) throw new InvalidOperationException("Game has ended");

        var move = new Move.Move(from, to);
        
        if (!_moveValidator.IsValidMove(_board, move, CurrentPlayer))
            return false;

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
        var randomMove = _moveGenerator.GenerateMove(_board, CurrentPlayer);
        if (randomMove == null) return;
        MakeMove(randomMove.From, randomMove.To);
    }

    public List<Move.Move> GetValidMoves() => _moveValidator.GetValidMoves(_board, CurrentPlayer);

    public List<Move.Move> GetValidMoves(PlayerType.PlayerType player) => 
        _moveValidator.GetValidMoves(_board, player);

    public Cell GetCell(Position.Position pos) => _board.GetCell(pos);

    public Cell[,] GetBoard() => _board.GetCells();

    public (int xCount, int oCount) GetPieceCounts() => _board.CountPieces();
    
    public GameState GetGameState()
    {
        var (xCount, oCount) = GetPieceCounts();
        var cells = new CellState[BoardSize, BoardSize];
    
        for (var row = 0; row < BoardSize; row++)
        {
            for (var col = 0; col < BoardSize; col++)
            {
                var cell = GetCell(new Position.Position(row, col));
                cells[row, col] = new CellState(cell.OccupiedBy, cell.IsBlocked);
            }
        }

        return new GameState(BoardSize, cells, CurrentPlayer, xCount,
            oCount, IsEnded, Winner, LayoutName);
    }

    private void ExecuteMove(Move.Move move)
    {
        _moveExecutor.ExecuteMove(_board, move, CurrentPlayer);
        SwitchPlayer();
        CheckGameEnd();
    }

    private void SwitchPlayer()
    {
        _progress.AdvanceTurn();
        
        var opponent = CurrentPlayer.GetOpponent();
        if (_moveValidator.GetValidMoves(_board, CurrentPlayer).Count == 0 && 
            _moveValidator.GetValidMoves(_board, opponent).Count > 0) 
            _progress.AdvanceTurn();
    }

    private void CheckGameEnd()
    {
        var result = _endDetector.CheckGameEnd(_board, _moveValidator, CurrentPlayer);
        
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
        new GameMemento(_board.Clone(), CurrentPlayer, TurnNumber);

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