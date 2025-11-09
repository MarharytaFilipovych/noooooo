using Command;
using Commands.CommandDefinition;

namespace Commands.CommandExecutor;

public class HelpCommandExecutor(List<ICommandDefinition> commands) : ICommandExecutor<HelpCommand>
{
    public ExecuteResult Execute(HelpCommand command)
    {
        commands.ForEach(c => Console.WriteLine($"• {c.Description}. Usage: {c.Usage}"));
        return ExecuteResult.Continue;
    }
}