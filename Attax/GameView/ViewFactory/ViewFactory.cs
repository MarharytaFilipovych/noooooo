using Model;
using View.Views;

namespace View.ViewFactory;

public class ViewFactory : IViewFactory
{
    private readonly Dictionary<ViewType, Func<IGameView>> _viewCreators = new();

    public IGameView CreateView(ViewType type) =>
        _viewCreators.TryGetValue(type, out var creator)
            ? creator() : throw new Exception($"No view was registered for the type {type}");
    
    public void RegisterView(ViewType type, Func<IGameView> creator) => 
        _viewCreators[type] = creator ?? throw new ArgumentNullException(nameof(creator));

    public IReadOnlyList<ViewType> GetAvailableViews() => _viewCreators.Keys.ToList();
}

/* private static readonly IBoardLayout[] Layouts =
    [
        new ClassicLayout(),
        new CrossLayout(),
        new CenterBlockLayout()
    ];*/