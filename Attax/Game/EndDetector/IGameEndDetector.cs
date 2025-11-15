using Move.Validator;

namespace Model.Game.EndDetector;

public interface IGameEndDetector
{
    GameEndResult CheckGameEnd(Board.Board board, IMoveValidator validator, PlayerType.PlayerType currentPlayer);
}

public readonly struct GameEndResult(bool isEnded, PlayerType.PlayerType winner)
{
    public bool IsEnded { get; } = isEnded;
    public PlayerType.PlayerType Winner { get; } = winner;
}
