namespace Model.Game.Mode;

public interface IGameModeFactory
{
    void RegisterMode(GameMode mode);
    IReadOnlyList<GameModeOption> GetAvailableModes();
}

public record GameModeOption(GameMode Mode, string DisplayName, string Description);
