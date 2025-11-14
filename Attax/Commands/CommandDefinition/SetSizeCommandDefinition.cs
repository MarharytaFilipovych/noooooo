using System.Runtime.InteropServices.JavaScript;
using Command;

namespace Commands.CommandDefinition;

public class SetSizeCommandDefinition : ICommandDefinition
{
    private const int MinBoardSize = 5;
    private const int MaxBoardSize = 20;

    public string Name => "set size";
    public string Description => "Set game board size!";
    public string Usage => "set size <number> (like \"set size 8\")";
    public bool TryParse(string[] args, out ICommand? command, out string? error)
    {
        command = null;
        error = null;

        if (args.Length < 3)
        {
            error = $"Wrong usage, bro: {Usage}";
            return false;
        }
        
        if (!int.TryParse(args[2], out var size))
        {
            error = $"'{args[2]}' is not a valid number! Do you know how numbers should look like, you dummy?";
            return false;
        }

        if (size is < MinBoardSize or > MaxBoardSize)
        {
            error = $"Board size must be between {MinBoardSize} and {MaxBoardSize}.";
            return false;
        }

        command = new SetSizeCommand(size);
        return true;
    }
}