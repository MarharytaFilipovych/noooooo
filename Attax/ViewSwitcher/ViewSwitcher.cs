using Model;
using View;
using View.ViewFactory;

namespace ViewSwitcher;

public class ViewSwitcher : IViewSwitcher
{
    private readonly IViewFactory _viewFactory;
    public IGameView CurrentView { get; private set; }
    public ViewType CurrentViewType { get; private set; }

    public ViewSwitcher(IViewFactory viewFactory, ViewType initialViewType = ViewType.Simple)
    {
        CurrentViewType = initialViewType;
        
        _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));

        if ( _viewFactory.GetAvailableViews().Count == 0) 
            throw new InvalidOperationException("No views registered");
        
        CurrentView = _viewFactory.CreateView(CurrentViewType);
    }

    public void SwitchView(ViewType viewType)
    {
        CurrentViewType = viewType;
        CurrentView = _viewFactory.CreateView(CurrentViewType);
        ViewSelected?.Invoke();
    }

    public event Action? ViewSelected;
}