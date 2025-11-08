using Attax.Presenters;

namespace Attax.Commands;

public class SwitchViewCommandHandler(GamePresenter presenter) : ICommandHandler
{
    private readonly GamePresenter _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));

    public bool CanHandle(string command) => command == "switch";

    public bool Execute(string[] parts)
    {
        _presenter.SwitchView();
        return true;
    }
}