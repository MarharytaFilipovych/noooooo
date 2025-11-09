using Model;
using View;
using View.ViewFactory;

namespace ViewSwitcher;

public class ViewSwitcher : IViewSwitcher
{

    private readonly IViewFactory _viewFactory;
    private readonly List<ViewType> _availableViews;

    public IGameView CurrentView { get; private set; }

    public ViewType CurrentViewType { get; private set; }

    public ViewSwitcher(IViewFactory viewFactory, ViewType initialViewType = ViewType.Simple)
    {
        CurrentViewType = initialViewType;
        
        _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
        _availableViews = _viewFactory.GetAvailableViews().ToList();

        if (_availableViews.Count == 0) throw new InvalidOperationException("No views registered");

        
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