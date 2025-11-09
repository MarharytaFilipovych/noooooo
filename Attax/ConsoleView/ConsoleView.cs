namespace ConsoleView;

public class ConsoleView
{
    private readonly IConsoleIO _console = console ?? throw new ArgumentNullException(nameof(console));

    public void DisplayMessage(string message)
    {
        _console.WriteLine($" {message}");
    }

    public string GetInput(string prompt)
    {
        _console.Write($"> {prompt}: ");
        return _console.ReadLine();
    }
}

