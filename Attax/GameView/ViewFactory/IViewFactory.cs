using Model;

namespace View.ViewFactory;

public interface IViewFactory
{
    IGameView CreateView(ViewType type);
    IReadOnlyList<ViewType> GetAvailableViews();
}