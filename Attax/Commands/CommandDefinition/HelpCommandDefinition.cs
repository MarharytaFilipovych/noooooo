using Command;
using GameMode;
using GameMode.ModeType;

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
    
    public bool IsAvailableInMode(GameModeType modeType) => 
        modeType is GameModeType.PvE or GameModeType.PvP;
}