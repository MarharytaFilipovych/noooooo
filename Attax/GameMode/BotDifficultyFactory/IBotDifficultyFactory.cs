using GameMode.ModeConfigurations;

namespace GameMode.BotDifficultyFactory;

public interface IBotDifficultyFactory
{
    void RegisterDifficulty(BotDifficultyOption option);
    IReadOnlyList<BotDifficultyOption> GetAvailableDifficulties();
    BotDifficultyOption GetDifficulty(BotDifficulty difficulty);
}

public record BotDifficultyOption(BotDifficulty Difficulty, string DisplayName, string Description);