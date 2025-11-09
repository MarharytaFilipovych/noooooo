using Command;

namespace Commands.CommandExecutor;

public interface ICommandExecutor<in TCommand> where TCommand : ICommand
{
    ExecuteResult Execute(TCommand command);
}