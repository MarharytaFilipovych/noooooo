namespace Commands.CommandExecutor;

using Command;
using Commands;
using Model.Game.Settings;

public class ChooseLayoutCommandExecutor(IGameSettings settings) : ICommandExecutor<ChooseLayoutCommand>
{
    public ExecuteResult Execute(ChooseLayoutCommand command)
    {
        try
        {
            settings.LayoutType = command.LayoutType;
            return ExecuteResult.Continue;
        }
        catch (InvalidOperationException)
        {
            return ExecuteResult.Error;
        }
    }
}