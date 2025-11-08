using Model.Game.EventPublisher;
using Model.Position;

namespace Model.Game.Game;

public class AtaxxGameWithEvents : AtaxxGame
{
    private readonly IGameEventPublisher _eventPublisher;

    public event Action<Cell[,], string>? GameStarted
    {
        add => _eventPublisher.GameStarted += value;
        remove => _eventPublisher.GameStarted -= value;
    }

    public event Action<PlayerType.PlayerType>? PlayerWon
    {
        add => _eventPublisher.PlayerWon += value;
        remove => _eventPublisher.PlayerWon -= value;
    }

    public event Action? GameDrawn
    {
        add => _eventPublisher.GameDrawn += value;
        remove => _eventPublisher.GameDrawn -= value;
    }

    public event Action<PlayerType.PlayerType>? TurnChanged
    {
        add => _eventPublisher.TurnChanged += value;
        remove => _eventPublisher.TurnChanged -= value;
    }

    public event Action<Move, PlayerType.PlayerType>? MoveMade
    {
        add => _eventPublisher.MoveMade += value;
        remove => _eventPublisher.MoveMade -= value;
    }

    public event Action<Move, PlayerType.PlayerType>? MoveInvalid
    {
        add => _eventPublisher.MoveInvalid += value;
        remove => _eventPublisher.MoveInvalid -= value;
    }

    public event Action<Cell[,]>? BoardUpdated
    {
        add => _eventPublisher.BoardUpdated += value;
        remove => _eventPublisher.BoardUpdated -= value;
    }

    public event Action<List<Move>>? HintRequested
    {
        add => _eventPublisher.HintRequested += value;
        remove => _eventPublisher.HintRequested -= value;
    }

    public AtaxxGameWithEvents(int boardSize = 7, IGameEventPublisher? publisher = null) 
        : base(boardSize)
    {
        _eventPublisher = publisher ?? new GameEventPublisher();
    }

    public AtaxxGameWithEvents(int boardSize, Board.IBoardLayout layout, IGameEventPublisher? publisher = null) 
        : base(boardSize, layout)
    {
        _eventPublisher = publisher ?? new GameEventPublisher();
    }


    public void StartGameWithEvents()
    {
        StartGame();
        _eventPublisher.PublishGameStart(GetBoard(), LayoutName, CurrentPlayer);
    }

    public bool MakeMoveWithEvents(Position.Position from, Position.Position to)
    {
        var move = new Move(from, to);
        var previousPlayer = CurrentPlayer;
        var success = MakeMove(from, to);
        
        _eventPublisher.PublishMoveResult(this, move, previousPlayer, success);
        return success;
    }

    public bool MakeMoveWithEvents(string fromNotation, string toNotation)
    {
        if (!PositionParser.TryParse(fromNotation, out var from)) return false;
        return PositionParser.TryParse(toNotation, out var to) && MakeMoveWithEvents(from, to);
    }

    public void RequestHint() => _eventPublisher.PublishHint(GetValidMoves());
}
