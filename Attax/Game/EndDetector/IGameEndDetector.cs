using Move.Validator;

namespace Model.Game.EndDetector;

public interface IGameEndDetector
{
    GameEndResult CheckGameEnd(Board.Board board, IMoveValidator validator, PlayerType.PlayerType currentPlayer);
}