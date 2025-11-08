using Model.Game;
using Model.Game.Mode;

namespace Attax;

public interface IGameModeSelector
{
    GameModeConfiguration SelectGameMode();
}