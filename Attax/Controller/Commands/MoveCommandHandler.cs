using Attax.Presenters;
using Model.Game;
using Model.Game.Game;

namespace Attax.Commands;

public class MoveCommandHandler(AtaxxGameWithEvents game, 
    GamePresenter presenter) : ICommandHandler
{
    private readonly AtaxxGameWithEvents _game = game ?? throw new ArgumentNullException(nameof(game));
    private readonly GamePresenter _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

    public bool CanHandle(string command) => command == "move";

    public bool Execute(string[] parts)
    {
        if (parts.Length < 3)
        {
            _presenter.DisplayMessage("Usage: move FROM TO (e.g., move A1 B2)");
            return true;
        }

        _game.MakeMoveWithEvents(parts[1], parts[2]);
        return true;
    }
}