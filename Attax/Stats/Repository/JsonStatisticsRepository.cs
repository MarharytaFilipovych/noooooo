using System.Text.Json;

namespace Stats.Repository;

public class JsonStatisticsRepository(string? filePath = null) : IStatisticsRepository
{
    private string FilePath => filePath ?? "statistics.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };


    public GameStatistics LoadStatistics()
    {
        if (!File.Exists(FilePath)) return GameStatistics.Empty;

        try
        {
            var json = File.ReadAllText(FilePath );
            return JsonSerializer.Deserialize<GameStatistics>(json) ?? GameStatistics.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading statistics: {ex.Message}");
            return GameStatistics.Empty;
        }
    }

    public void SaveStatistics(GameStatistics statistics)
    {
        try
        {
            var json = JsonSerializer.Serialize(statistics, JsonOptions);
            File.WriteAllText(filePath!, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving statistics: {ex.Message}");
        }
    }

    public void ResetStatistics()
    {
        if (File.Exists(FilePath )) File.Delete(FilePath );
    }
}