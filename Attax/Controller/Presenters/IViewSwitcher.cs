using View;

namespace Attax.Presenters;

public interface IViewSwitcher
{
    IGameView CurrentView { get; }
    ViewType CurrentViewType { get; }
    void SwitchView();
}