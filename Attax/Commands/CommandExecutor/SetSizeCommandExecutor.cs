using Command;
using Model.Game.Game;
using Model.Game.Settings;

namespace Commands.CommandExecutor;

public class SetSizeCommandExecutor(IGameSettings settings) : ICommandExecutor<SetSizeCommand>
{
    public ExecuteResult Execute(SetSizeCommand command)
    {
        try
        {
            settings.BoardSize = command.Size;
            return ExecuteResult.Continue;
        }
        catch (InvalidOperationException)
        {
            return ExecuteResult.Error;
        }
        
    }
}