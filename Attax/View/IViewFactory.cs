namespace View;

public interface IViewFactory
{
    IGameView CreateView(ViewType type);
}