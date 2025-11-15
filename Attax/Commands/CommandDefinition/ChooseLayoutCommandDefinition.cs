using Command;
using Layout.LayoutType;
using ICommand = System.Windows.Input.ICommand;

namespace Commands.CommandDefinition;

public class ChooseLayoutCommandDefinition : ICommandDefinition
{
    public string Name => "layout";
    public string Description => $"Choose board layout ({LayoutTypeParser.AllValidDescriptions()})";
    public string Usage => "layout <type> (e.g., \"layout classic\" or \"layout cross\" or \"layout center-block\")";

    public bool TryParse(string[] args, out Command.ICommand? command, out string? error)
    {
        command = null;
        error = null;

        if (args.Length < 2)
        {
            error = $"Wrong usage, bro: {Usage}";
            return false;
        }

        var layoutInput = args[1];

        if (!LayoutTypeParser.TryParse(layoutInput, out var layoutType))
        {
            error = $"We do not recognize this layout type: \"{args[1]}\"." +
                    $"You must choose from: {LayoutTypeParser.AllValidDescriptions()}";
            return false;
        }

        command = new ChooseLayoutCommand(layoutType);
        return true;
    }

}


