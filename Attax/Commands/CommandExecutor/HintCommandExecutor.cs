using Command;
using Model.Game.Game;

namespace Commands.CommandExecutor;

public class HintCommandExecutor(AtaxxGameWithEvents game) : ICommandExecutor<HintCommand>
{
    private readonly AtaxxGameWithEvents _game = game ?? throw new ArgumentNullException();
    
    public ExecuteResult Execute(HintCommand command)
    {
        _game.ShowHint();
        return ExecuteResult.Continue;
    }
}