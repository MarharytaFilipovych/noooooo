namespace Stats;

public class StatisticsOptions
{
    private const string DefaultFileName = "statistics";
    private const string DefaultDirectoryName = "BeautifulStats";

    public string FileName { get; set; } = DefaultFileName;
    public string DirectoryName { get; set; } = DefaultDirectoryName;
}
