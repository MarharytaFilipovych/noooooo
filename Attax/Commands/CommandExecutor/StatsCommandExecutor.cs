using Command;

namespace Commands.CommandExecutor;

// to do
public class StatsCommandExecutor : ICommandExecutor<StatsCommand>
{
    public ExecuteResult Execute(StatsCommand command)
    {
        return ExecuteResult.Continue;
    }
}