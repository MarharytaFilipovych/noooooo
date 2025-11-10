using Command;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class UndoCommandExecutor(AtaxxGameWithEvents game) : ICommandExecutor<UndoCommand>
{
    public ExecuteResult Execute(UndoCommand command)
    {
        game.UndoLastMove();
        return ExecuteResult.Continue;
    }
}