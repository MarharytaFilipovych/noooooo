using Model.Game.Mode;

namespace ModeSelector;


public interface IModeSelector
{
    GameModeConfiguration SelectGameMode();
}