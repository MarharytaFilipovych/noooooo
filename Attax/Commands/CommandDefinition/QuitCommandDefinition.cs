using Command;

namespace Commands.CommandDefinition;

public class QuitCommandDefinition : ICommandDefinition
{
    public string Name => "quit";
    public string Description => "Exit the game and then come back in the nearest future:))))";
    public string Usage => "quit";

    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new QuitCommand();
        error = null;
        return true;
    }
}