using Command;
using Commands.CommandDefinition;
using Commands.CommandExecutor;

namespace Commands;

public interface ICommandProcessor
{
    public void Register<TCommand>(ICommandDefinition commandDefinition, ICommandExecutor<TCommand> executor)
        where TCommand : ICommand;

    public IReadOnlyList<ICommandDefinition> Commands();
    
    ExecuteResult ProcessCommand(string[] args, out string? error);
}