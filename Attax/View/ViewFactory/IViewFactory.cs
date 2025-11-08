namespace View.ViewFactory;

public interface IViewFactory
{
    IGameView CreateView(ViewType type);
}