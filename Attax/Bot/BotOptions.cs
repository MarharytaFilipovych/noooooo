namespace Bot;

public class BotOptions
{
    private const int DefaultThinkingDelayMs = 500;
    public int ThinkingDelayMs { get; set; } = DefaultThinkingDelayMs;
}
