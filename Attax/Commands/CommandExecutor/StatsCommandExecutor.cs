using Command;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class StatsCommandExecutor(AtaxxGameWithEvents game) : ICommandExecutor<StatsCommand>
{
    public ExecuteResult Execute(StatsCommand command)
    {
        game.DisplayStats();
        return ExecuteResult.Continue;
    }
}