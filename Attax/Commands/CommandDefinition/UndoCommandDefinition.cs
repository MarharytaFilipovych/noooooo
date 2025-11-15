using Command;
using Model.Game.Mode;

namespace Commands.CommandDefinition;

public class UndoCommandDefinition : ICommandDefinition
{
    public string Name => "undo";

    public string Description => "Undo your previous move, darling❤️. " +
                                 "Remember, you have to preform this action within 3-second window!";

    public string Usage => "undo";
    
    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new UndoCommand();
        error = null;
        return true;
    }
    
    public bool IsAvailableInMode(GameMode mode) => mode is GameMode.PvE;
}