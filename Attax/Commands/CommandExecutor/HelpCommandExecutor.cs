using Command;
using Commands.CommandDefinition;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class HelpCommandExecutor(AtaxxGameWithEvents game, List<ICommandDefinition> commands) 
    : ICommandExecutor<HelpCommand>
{
    public ExecuteResult Execute(HelpCommand command)
    {
        var availableCommands = commands
            .Select(c => (c.Name, c.Usage, c.Description))
            .ToList();
    
        game.RequestHelp(availableCommands);
        return ExecuteResult.Continue;
    }
}