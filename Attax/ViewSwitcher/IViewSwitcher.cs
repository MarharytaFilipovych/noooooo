using Model;
using View;
using View.Views;

namespace ViewSwitcher;

public interface IViewSwitcher
{
    IGameView CurrentView { get; }
    ViewType CurrentViewType { get; }
    void SwitchView(ViewType viewType);
    event Action? ViewSelected; 
}