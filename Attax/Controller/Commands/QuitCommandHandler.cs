namespace Controller.Commands;

public class QuitCommandHandler : ICommandHandler
{
    public bool CanHandle(string command) => command is "quit" or "exit";

    public bool Execute(string[] parts) => false;
}