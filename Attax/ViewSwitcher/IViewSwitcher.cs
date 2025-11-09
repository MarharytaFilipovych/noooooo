using Model;
using View;

namespace ViewSwitcher;

public interface IViewSwitcher
{
    IGameView CurrentView { get; }
    ViewType CurrentViewType { get; }
    void SwitchView(ViewType viewType);
    event Action? ViewSelected; 
}