namespace Controller.Presenters;

using View;
using System;

public class ViewSwitcher : IViewSwitcher
{
    private readonly IViewFactory _viewFactory;
    private ViewType _currentViewType;
    private IGameView _currentView;

    public IGameView CurrentView => _currentView;
    public ViewType CurrentViewType => _currentViewType;

    public ViewSwitcher(IViewFactory viewFactory, ViewType initialViewType = ViewType.Simple)
    {
        _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
        _currentViewType = initialViewType;
        _currentView = _viewFactory.CreateView(_currentViewType);
    }

    public void SwitchView()
    {
        _currentViewType = _currentViewType == ViewType.Simple 
            ? ViewType.Enhanced 
            : ViewType.Simple;
        
        _currentView = _viewFactory.CreateView(_currentViewType);
    }
}