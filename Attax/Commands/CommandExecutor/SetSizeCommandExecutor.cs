using Command;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class SetSizeCommandExecutor(AtaxxGameWithEvents game) : ICommandExecutor<SetSizeCommand>
{
    public ExecuteResult Execute(SetSizeCommand command)
    {
        return ExecuteResult.Continue;
    }
}