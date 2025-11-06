namespace Controller.Presenters;

using View;

public interface IViewSwitcher
{
    IGameView CurrentView { get; }
    ViewType CurrentViewType { get; }
    void SwitchView();
}