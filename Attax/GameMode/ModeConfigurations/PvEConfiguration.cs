using GameMode.ModeType;
using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public class PvEConfiguration : IGameModeConfiguration
{
    public PvEConfiguration(PlayerType humanPlayer, BotDifficulty botDifficulty)
    {
        if (humanPlayer == PlayerType.None)
            throw new ArgumentException("Human player must be X or O", nameof(humanPlayer));
        
        HumanPlayer = humanPlayer;
        BotPlayer = humanPlayer.GetOpponent();
        BotDifficulty = botDifficulty;
    }

    public GameModeType ModeType => GameModeType.PvE;
    public bool IsBot(PlayerType player) => player == BotPlayer;
    
    public PlayerType HumanPlayer { get; }
    public PlayerType BotPlayer { get; }
    public BotDifficulty BotDifficulty { get; }
}