using Model.Game.Game;

namespace Model.Game.EventPublisher;

public class GameEventPublisher : IGameEventPublisher
{
    public event Action<Cell[,], string>? GameStarted;
    public event Action<PlayerType.PlayerType>? PlayerWon;
    public event Action? GameDrawn;
    public event Action<PlayerType.PlayerType>? TurnChanged;
    public event Action<Move, PlayerType.PlayerType>? MoveMade;
    public event Action<Move, PlayerType.PlayerType>? MoveInvalid;
    public event Action<Cell[,]>? BoardUpdated;
    public event Action<List<Move>>? HintRequested;

    public void PublishGameStart(Cell[,] board, string layoutName, PlayerType.PlayerType currentPlayer)
    {
        GameStarted?.Invoke(board, layoutName);
        TurnChanged?.Invoke(currentPlayer);
    }

    public void PublishMoveResult(AtaxxGame game, Move move, PlayerType.PlayerType previousPlayer, bool success)
    {
        if (success)
        {
            MoveMade?.Invoke(move, previousPlayer);
            BoardUpdated?.Invoke(game.GetBoard());
            
            if (!game.IsEnded) TurnChanged?.Invoke(game.CurrentPlayer);
            else PublishGameEnd(game);
        }
        else MoveInvalid?.Invoke(move, previousPlayer);
    }

    public void PublishHint(List<Move> validMoves) => HintRequested?.Invoke(validMoves);

    private void PublishGameEnd(AtaxxGame game)
    {
        if (game.Winner == PlayerType.PlayerType.None) GameDrawn?.Invoke();
        else PlayerWon?.Invoke(game.Winner);
    }
}
