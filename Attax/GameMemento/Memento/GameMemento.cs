using Model.Board;
using Model.PlayerType;

namespace GameMemento.Memento;

public class GameMemento(Board board, PlayerType currentPlayer,
    int turnNumber, DateTime timestamp) : IMemento
{
    public Board Board { get; } = board;
    public PlayerType CurrentPlayer { get; } = currentPlayer;
    public int TurnNumber { get; } = turnNumber;
    public DateTime Timestamp { get; } = timestamp;
}