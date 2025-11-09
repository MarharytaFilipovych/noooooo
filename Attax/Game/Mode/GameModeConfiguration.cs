using Model.PlayerType;

namespace Model.Game.Mode;

public sealed record GameModeConfiguration(GameMode Mode,
    PlayerType.PlayerType? HumanPlayer = null, PlayerType.PlayerType? BotPlayer = null
)
{
    public bool IsBot(PlayerType.PlayerType player) =>
        Mode == GameMode.PvE && player == BotPlayer;

    public static GameModeConfiguration CreatePvP() =>
        new(GameMode.PvP);

    public static GameModeConfiguration CreatePvE(PlayerType.PlayerType humanPlayer)
    {
        return humanPlayer == PlayerType.PlayerType.None 
            ? throw new ArgumentException("Human player must be X or O", nameof(humanPlayer))
            : new GameModeConfiguration(GameMode.PvE, humanPlayer, humanPlayer.GetOpponent());
    }
}