using Command;
using Model;
using ViewSwitcher;

namespace Commands.CommandExecutor;

public class SwitchViewCommandExecutor(IViewSwitcher viewSwitcher) 
    : ICommandExecutor<SwitchViewCommand>
{

    public ExecuteResult Execute(SwitchViewCommand command)
    {
        var newType = viewSwitcher.CurrentViewType == ViewType.Enhanced
            ? ViewType.Simple : ViewType.Enhanced;
        viewSwitcher.SwitchView(newType);
        return ExecuteResult.Continue;
    }
}