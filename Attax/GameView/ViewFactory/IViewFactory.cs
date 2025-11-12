using Model;
using View.Views;

namespace View.ViewFactory;

public interface IViewFactory
{
    IGameView CreateView(ViewType type);
    IReadOnlyList<ViewType> GetAvailableViews();
    void RegisterView(ViewType type, Func<IGameView> creator);
}