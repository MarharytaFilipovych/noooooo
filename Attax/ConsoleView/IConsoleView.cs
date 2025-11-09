namespace ConsoleView;

public interface IConsoleView
{
    void DisplayMessage(string message);
    string GetInput(string prompt);
}