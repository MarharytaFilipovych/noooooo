namespace Controller.Commands;

using Controller.Presenters;
using System;

public class SwitchViewCommandHandler : ICommandHandler
{
    private readonly GamePresenter _presenter;

    public SwitchViewCommandHandler(GamePresenter presenter)
    {
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
    }

    public bool CanHandle(string command) => command == "switch";

    public bool Execute(string[] parts)
    {
        _presenter.SwitchView();
        return true;
    }
}