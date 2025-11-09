using Command;

namespace Commands.CommandDefinition;

public class SwitchViewCommandDefinition : ICommandDefinition
{
    public string Name => "switch";
    public string Description => "Switch between simple and enhanced views " +
                                 "so that your experience would be more beautiful....";
    public string Usage => "switch";

    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = new SwitchViewCommand();
        error = null;
        return true;
    }
}