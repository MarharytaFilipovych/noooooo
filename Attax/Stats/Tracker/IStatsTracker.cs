using Model.PlayerType;

namespace Stats.Tracker;

public interface IStatsTracker
{
    public void RecordGame(PlayerType winner, int turnCount);
    public GameStatistics GetStatistics();
}