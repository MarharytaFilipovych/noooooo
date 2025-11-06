namespace Controller.Commands;

using Model.Game;
using Controller.Presenters;
using System;

public class MoveCommandHandler : ICommandHandler
{
    private readonly AtaxxGameWithEvents _game;
    private readonly GamePresenter _presenter;

    public MoveCommandHandler(AtaxxGameWithEvents game, GamePresenter presenter)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

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