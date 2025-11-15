using Command;
using Model.Game.Mode;

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
    
    public bool IsAvailableInMode(GameMode mode) => mode is GameMode.PvE or GameMode.PvP;
}