using Command;
using GameMode;

namespace Commands.CommandDefinition;

public class HintCommandDefinition : ICommandDefinition
{
    public string Name => "hint";
    public string Description => "Show all valid moves for current player";
    public string Usage => "hint";

    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new HintCommand();
        error = null;
        return true;
    }
    
    public bool IsAvailableInMode(ModeType modeType) => 
        modeType is ModeType.PvE or ModeType.PvP;
}