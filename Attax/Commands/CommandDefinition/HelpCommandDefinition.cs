using Command;
using Model.Game.Mode;

namespace Commands.CommandDefinition;

public class HelpCommandDefinition : ICommandDefinition
{
    public string Name => "help";
    public string Description => "Help yourself by looking at the detailed list of provided commands:)";
    public string Usage => "help";
    

    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new HelpCommand();
        error = null;
        return true;
    }
    
    public bool IsAvailableInMode(GameMode mode) => mode is GameMode.PvE or GameMode.PvP;
}