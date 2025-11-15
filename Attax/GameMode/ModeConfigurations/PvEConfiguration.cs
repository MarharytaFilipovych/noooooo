using GameMode.ModeType;
using Model.PlayerType;

namespace GameMode.ModeConfigurations;

public class PvEConfiguration : IGameModeConfiguration
{
    private PlayerType BotPlayer { get; }

    public PvEConfiguration(PlayerType humanPlayer)
    {
        if (humanPlayer == PlayerType.None)
            throw new ArgumentException("Human player must be X or O", nameof(humanPlayer));
        
        HumanPlayer = humanPlayer;
        BotPlayer = humanPlayer.GetOpponent();
    }

    public GameModeType ModeType => GameModeType.PvE;
    public bool IsBot(PlayerType player) => player == BotPlayer;
    
    public PlayerType HumanPlayer { get; }
}