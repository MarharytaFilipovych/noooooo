namespace Controller.Commands;

public interface ICommandHandler
{
    bool CanHandle(string command);
    bool Execute(string[] parts);
}