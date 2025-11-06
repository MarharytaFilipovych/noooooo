namespace Controller;

using Model.Game;

public interface IGameModeSelector
{
    GameModeConfiguration SelectGameMode();
}