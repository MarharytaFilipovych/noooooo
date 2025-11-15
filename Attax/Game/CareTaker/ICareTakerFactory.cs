using Model.Game.Game;

namespace Model.Game.CareTaker;

public interface ICareTakerFactory
{
    ICareTaker Create(AtaxxGame game);
}