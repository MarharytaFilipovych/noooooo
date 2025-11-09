namespace Model.Game.TurnTimer;

public interface ITurnTimer
{
    void StartTurn();
    void StopTurn();
    void ResetTurn();
    event Action? TimeoutOccurred;
}