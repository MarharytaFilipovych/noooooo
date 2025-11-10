using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stats.Repository;

public class JsonStatisticsRepository : IStatisticsRepository
{
    private readonly string? _filePath;
    private const string FileName = "statistics.json";
    private const string DirectoryName = "BeautifulStats";

    public JsonStatisticsRepository(string? filePath = null)
    {
        if (!Directory.Exists(DirectoryName)) Directory.CreateDirectory(DirectoryName);

        _filePath = GetFilePath(filePath ?? FileName);
    }

    public GameStatistics LoadStatistics()
    {
        if (!File.Exists(_filePath)) return GameStatistics.Empty;

        try
        {
            var json = File.ReadAllText(_filePath);
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
            File.WriteAllText(_filePath!, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving statistics: {ex.Message}");
        }
    }

    public void ResetStatistics()
    {
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    { 
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    private static string GetFilePath(string filepath) => 
        Path.Combine(GetProjectRoot(), DirectoryName, $"{filepath}.json");
    
    private static string GetProjectRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        var projectName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        var csprojFile = $"{projectName}.csproj";

        while (directory != null && !File.Exists(Path.Combine(directory.FullName, csprojFile)))
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? AppContext.BaseDirectory;
    }
}

