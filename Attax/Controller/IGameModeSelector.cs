using Model.Game;

namespace Attax;

public interface IGameModeSelector
{
    GameModeConfiguration SelectGameMode();
}