using Command;

namespace Commands.CommandExecutor;

public class QuitCommandExecutor : ICommandExecutor<QuitCommand>
{
    public ExecuteResult Execute(QuitCommand command) => ExecuteResult.Break;
}