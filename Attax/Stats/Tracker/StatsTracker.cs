using Model.PlayerType;
using Stats.Repository;

namespace Stats.Tracker;

public class StatsTracker : IStatsTracker
{
    private readonly IStatisticsRepository _repository;
    private GameStatistics _statistics;

    public StatsTracker(IStatisticsRepository repository)
    {
        _repository = repository;
        _statistics = _repository.LoadStatistics();
    }

    public void RecordGame(PlayerType winner, int turnCount)
    {
        _statistics = _statistics.AddGame(winner, turnCount);
        _repository.SaveStatistics(_statistics);
    }

    public GameStatistics GetStatistics() => _statistics;
}