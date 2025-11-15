using Command;
using GameMode;
using GameMode.ModeType;

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
    
    public bool IsAvailableInMode(GameModeType modeType) => 
        modeType is GameModeType.PvE or GameModeType.PvP;
}