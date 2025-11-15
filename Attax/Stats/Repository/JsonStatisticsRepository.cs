using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stats.Repository;

public class JsonStatisticsRepository(StatisticsOptions options) : IStatisticsRepository
{
    private readonly string _filePath = GetFilePath(options.FileName, options.DirectoryName);

    public GameStatistics LoadStatistics()
    {
        EnsureDirectoryExists(options.DirectoryName);
        
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
        EnsureDirectoryExists(options.DirectoryName);
        
        try
        {
            var json = JsonSerializer.Serialize(statistics, JsonOptions);
            File.WriteAllText(_filePath, json);
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
    
    private static void EnsureDirectoryExists(string directoryName)
    {
        if (!Directory.Exists(directoryName)) 
            Directory.CreateDirectory(directoryName);
    }
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    { 
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
    
    private static string GetFilePath(string fileName, string directoryName) => 
        Path.Combine(GetProjectRoot(), directoryName, $"{fileName}.json");
    
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


