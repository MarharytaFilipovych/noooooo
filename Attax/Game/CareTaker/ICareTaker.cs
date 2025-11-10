namespace Model.Game.CareTaker;

public interface ICareTaker
{
    void BackUp();
    bool Undo(int timeWindowSeconds);
}
