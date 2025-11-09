namespace Model.Game.TurnTimer;

using System.Timers;


public class TurnTimer : IDisposable, ITurnTimer
{
    private const int TimeoutSeconds = 20;
    private readonly Timer _timer;
    private readonly int _timeoutMilliseconds;
    private bool _isRunning;
    public event Action? TimeoutOccurred;

    public TurnTimer(int timeoutSeconds = TimeoutSeconds)
    {
        _timeoutMilliseconds = timeoutSeconds * 1000;
        _timer = new Timer();
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = false;
    }

    public void StartTurn()
    {
        if (_isRunning) return;
        
        _isRunning = true;
        _timer.Interval = _timeoutMilliseconds;
        _timer.Start();
    }

    public void StopTurn()
    {
        _isRunning = false;
        _timer.Stop();
    }

    public void ResetTurn()
    {
        StopTurn();
        StartTurn();
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _isRunning = false;
        TimeoutOccurred?.Invoke();
    }

    public void Dispose() => _timer.Dispose();
}