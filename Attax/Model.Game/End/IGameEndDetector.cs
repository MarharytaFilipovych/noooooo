using Model.Board;

namespace Model.Game.End;

public interface IGameEndDetector
{
    GameEndResult CheckGameEnd(Board.Board board, MoveValidator validator, PlayerType.PlayerType currentPlayer);

}

public readonly struct GameEndResult(bool isEnded, PlayerType.PlayerType winner)
{
    public bool IsEnded { get; } = isEnded;
    public PlayerType.PlayerType Winner { get; } = winner;
}
