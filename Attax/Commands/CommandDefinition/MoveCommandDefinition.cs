using Command;
using GameMode;
using Model.Position;

namespace Commands.CommandDefinition;

public class MoveCommandDefinition : ICommandDefinition
{
    public string Name => "move";
    public string Description => "Move a piece from one position to another:))";
    public string Usage => "\"move <FROM> <TO>\" (sth like \"move A1 B2\")";

    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = null;
        error = null;

        if (args.Length < 3)
        {
            error = $"Wrong usage, bro: {Usage}";
            return false;
        }

        if (!PositionParser.TryParse(args[1], out var from))
        {
            error = $"Are you able to provide correct FROM position??? What is that: \"{args[1]}\"?";
            return false;
        }

        if (!PositionParser.TryParse(args[2], out var to))
        {
            error = $"Are you able to provide correct TO position??? What is that: \"{args[2]}\"?";
            return false;
        }

        command = new MoveCommand(from, to);
        return true;
    }
    
    public bool IsAvailableInMode(ModeType modeType) => 
        modeType is ModeType.PvE or ModeType.PvP;
}