namespace Model.Game.Mode;

public interface IGameModeFactory
{
    GameModeConfiguration RegisterMode(string modeKey);
    IReadOnlyList<GameModeOption> GetAvailableModes();
}

public record GameModeOption(string Key, string DisplayName, string Description);
