using Model.PlayerType;

namespace Stats;

public sealed record GameStatistics(int GamesPlayed, int PlayerXWins,
    int PlayerOWins, int Draws, double AverageMoveCount, DateTime LastPlayed)
{
    public static GameStatistics Empty => 
        new(0, 0, 0, 0, 
            0, DateTime.MinValue);
    
    public GameStatistics AddGame(PlayerType winner, int moveCount)
    {
        return new GameStatistics(GamesPlayed: GamesPlayed + 1, 
            PlayerXWins: winner == PlayerType.X ? PlayerXWins + 1 : PlayerXWins, 
            PlayerOWins: winner == PlayerType.O ? PlayerOWins + 1 : PlayerOWins, 
            Draws: winner == PlayerType.None ? Draws + 1 : Draws, 
            AverageMoveCount: (AverageMoveCount * GamesPlayed + moveCount) / (GamesPlayed + 1),
            LastPlayed: DateTime.UtcNow);
    }
}