using Layout.Layout;

namespace Layout.Factory;

public interface IBoardLayoutFactory
{
    void RegisterLayout(IBoardLayout layout);
    IBoardLayout GetRandomLayout(Random? random = null);
    IBoardLayout GetLayout(LayoutType type);
    int GetLayoutCount();
    IReadOnlyList<LayoutType> GetAvailableLayouts();
}