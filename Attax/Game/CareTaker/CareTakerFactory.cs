using Model.Game.Game;

namespace Model.Game.CareTaker;

public class CareTakerFactory : ICareTakerFactory
{
    public ICareTaker Create(AtaxxGame game) => new CareTaker(game);
}