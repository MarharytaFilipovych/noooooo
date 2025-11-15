namespace Model.Game.Mode;


public interface IGameModeFactory
{
    void RegisterMode(GameModeOption option, Func<GameModeConfiguration> creator);
    GameModeConfiguration CreateMode(GameMode mode);
    IReadOnlyList<GameModeOption> GetAvailableModes();
}

public record GameModeOption(GameMode Mode, string DisplayName, string Description);