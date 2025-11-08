namespace Model.Game;

using System;

public sealed record GameModeConfiguration(
    GameMode Mode,
    PlayerType? HumanPlayer = null,
    PlayerType? BotPlayer = null
)
{
    public bool IsBot(PlayerType player) =>
        Mode == GameMode.PvE && player == BotPlayer;

    public static GameModeConfiguration CreatePvP() =>
        new(GameMode.PvP);

    public static GameModeConfiguration CreatePvE(PlayerType humanPlayer)
    {
        return humanPlayer == PlayerType.None 
            ? throw new ArgumentException("Human player must be X or O", nameof(humanPlayer))
            : new GameModeConfiguration(GameMode.PvE, humanPlayer, humanPlayer.GetOpponent());
    }
}