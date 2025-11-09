using View;
using View.ViewFactory;

namespace Attax.Presenters;

public class ViewSwitcher : IViewSwitcher
{
    private readonly IViewFactory _viewFactory;

    public IGameView CurrentView { get; private set; }

    public ViewType CurrentViewType { get; private set; }

    public ViewSwitcher(IViewFactory viewFactory, ViewType initialViewType = ViewType.Simple)
    {
        _viewFactory = viewFactory ?? throw new ArgumentNullException(nameof(viewFactory));
        CurrentViewType = initialViewType;
        CurrentView = _viewFactory.CreateView(CurrentViewType);
    }

    public void SwitchView()
    {
        CurrentViewType = CurrentViewType == ViewType.Simple 
            ? ViewType.Enhanced 
            : ViewType.Simple;
        
        CurrentView = _viewFactory.CreateView(CurrentViewType);
    }
}