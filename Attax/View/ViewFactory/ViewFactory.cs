namespace View.ViewFactory;

public class ViewFactory : IViewFactory
{
    private readonly Dictionary<ViewType, Func<IGameView>> _viewCreators = new()
    {
        { ViewType.Simple, () => new SimpleView() },
        { ViewType.Enhanced, () => new EnhancedView() }
    };

    public IGameView CreateView(ViewType type) =>
        _viewCreators.TryGetValue(type, out var creator)
            ? creator() : new SimpleView();

    public void RegisterView(ViewType type, Func<IGameView> creator) => 
        _viewCreators[type] = creator ?? throw new ArgumentNullException(nameof(creator));
}