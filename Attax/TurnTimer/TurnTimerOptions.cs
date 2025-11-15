namespace TurnTimer;

public class TurnTimerOptions
{
    private const int DefaultTimeoutSeconds = 20;
    public int TimeoutSeconds { get; set; } = DefaultTimeoutSeconds;
}
