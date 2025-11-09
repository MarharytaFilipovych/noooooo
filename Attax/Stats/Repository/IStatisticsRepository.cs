namespace Stats.Repository;

public interface IStatisticsRepository
{
    GameStatistics LoadStatistics();
    void SaveStatistics(GameStatistics statistics);
    void ResetStatistics();
}