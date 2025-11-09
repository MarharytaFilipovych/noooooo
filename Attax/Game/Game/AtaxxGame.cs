using Model.Board;
using Model.Board.Layouts;
using Model.Game.DTOs;
using Model.Game.Mode;
using Model.Game.TurnTimer;
using Model.PlayerType;
using Stats;
using Stats.Repository;

namespace Model.Game.Game;

public class AtaxxGame
{
    protected const int DefaultBoardSize = 7;
    protected GameStatistics Statistics;
    private readonly Board.Board _board;
    private readonly ITurnTimer _turnTimer;
    private readonly IBoardLayout _layout;
    private readonly MoveValidator _moveValidator;
    private readonly MoveExecutor _moveExecutor;
    private readonly GameEndDetector _endDetector;
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly Random _random;
    
    public GameModeConfiguration GameMode { get; set; } = GameModeConfiguration.CreatePvP();
    public PlayerType.PlayerType CurrentPlayer { get; private set; }
    public PlayerType.PlayerType Winner { get; private set; }
    public bool IsEnded { get; private set; }
    public int TurnNumber { get; private set; }
    
    public int BoardSize => _board.Size;
    public string LayoutName => _layout.Name;

    public AtaxxGame(IStatisticsRepository statisticsRepository, ITurnTimer turnTimer, 
        int boardSize = DefaultBoardSize)
    {
        _statisticsRepository = statisticsRepository;
        Statistics = _statisticsRepository.LoadStatistics();
        _turnTimer = turnTimer;
        _board = new Board.Board(boardSize);
        _layout = BoardLayoutFactory.GetRandomLayout();
        _moveValidator = new MoveValidator(_board);
        _moveExecutor = new MoveExecutor(_board);
        _endDetector = new GameEndDetector();
        _random = new Random();
        
        _turnTimer.TimeoutOccurred += HandleTimeout;
    }

    public AtaxxGame(IStatisticsRepository statisticsRepository, ITurnTimer turnTimer, 
        int boardSize, IBoardLayout boardLayout)
    {
        _board = new Board.Board(boardSize);
        _statisticsRepository = statisticsRepository;
        Statistics = _statisticsRepository.LoadStatistics();
        _turnTimer = turnTimer;
        _layout = boardLayout;
        _moveValidator = new MoveValidator(_board);
        _moveExecutor = new MoveExecutor(_board);
        _endDetector = new GameEndDetector();
        _random = new Random();
        
        _turnTimer.TimeoutOccurred += HandleTimeout;
        
    }

    private AtaxxGame(IStatisticsRepository statisticsRepository, ITurnTimer turnTimer,
        Board.Board clonedBoard, IBoardLayout boardLayout)
    {
        _statisticsRepository = statisticsRepository;
        Statistics = _statisticsRepository.LoadStatistics();
        _turnTimer = turnTimer;
        _board = clonedBoard;
        _layout = boardLayout;
        _moveValidator = new MoveValidator(_board);
        _moveExecutor = new MoveExecutor(_board);
        _endDetector = new GameEndDetector();
        _random = new Random();
        
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
        CurrentPlayer = PlayerType.PlayerType.X;
        Winner = PlayerType.PlayerType.None;
        IsEnded = false;
        TurnNumber = 1;
        _turnTimer.StartTurn();
    }

    public virtual bool MakeMove(Position.Position from, Position.Position to)
    {
        if (IsEnded) throw new InvalidOperationException("Game has ended");

        var move = new Move(from, to);
        
        if (!_moveValidator.IsValidMove(move, CurrentPlayer)) return false;

        ExecuteMove(move);
        
        if (!IsEnded)
        {
            _turnTimer.ResetTurn();
        }
        else
        {
            _turnTimer.StopTurn();
        }
        
        return true;
    }

    private void MakeRandomMove()
    {
        var validMoves = GetValidMoves();
        
        if (validMoves.Count == 0)
        {
            _turnTimer.StopTurn();
            return;
        }

        var randomMove = validMoves[_random.Next(validMoves.Count)];
        MakeMove(randomMove.From, randomMove.To);
    }

    public List<Move> GetValidMoves() => _moveValidator.GetValidMoves(CurrentPlayer);

    public List<Move> GetValidMoves(PlayerType.PlayerType player) => _moveValidator.GetValidMoves(player);

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

    private void ExecuteMove(Move move)
    {
        _moveExecutor.ExecuteMove(move, CurrentPlayer);
        SwitchPlayer();
        CheckGameEnd();
    }

    private void SwitchPlayer()
    {
        var nextPlayer = CurrentPlayer.GetOpponent();
        CurrentPlayer = nextPlayer;
        TurnNumber++;
        
        var opponent = CurrentPlayer.GetOpponent();
        if (_moveValidator.GetValidMoves(CurrentPlayer).Count == 0 && 
            _moveValidator.GetValidMoves(opponent).Count > 0)
        {
            CurrentPlayer = opponent;
        }
    }

    private void CheckGameEnd()
    {
        var result = _endDetector.CheckGameEnd(_board, _moveValidator, CurrentPlayer);
        
        if (result.IsEnded)
        {
            IsEnded = true;
            Winner = result.Winner;
            _turnTimer.StopTurn();
            Statistics = Statistics.AddGame(Winner, TurnNumber);
            _statisticsRepository.SaveStatistics(Statistics);
        }
    }
}