using Command;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class MoveCommandExecutor(AtaxxGame game) : ICommandExecutor<MoveCommand>
{
    private readonly AtaxxGame _game = game ?? throw new ArgumentNullException(nameof(game));

    public ExecuteResult Execute(MoveCommand command)
    {
        var success = _game.MakeMove(command.From, command.To);
        return success ? ExecuteResult.Continue : ExecuteResult.Error;
    }
}