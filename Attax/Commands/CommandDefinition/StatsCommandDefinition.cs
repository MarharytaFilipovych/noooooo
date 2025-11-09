using Command;

namespace Commands.CommandDefinition;

public class StatsCommandDefinition : ICommandDefinition
{
    public string Name => "stats";
    public string Description => "Observe game statistics!!!";
    public string Usage => "stats";
    
    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new StatsCommand();
        error = null;
        return true;
    }
}