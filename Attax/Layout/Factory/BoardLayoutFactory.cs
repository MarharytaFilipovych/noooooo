using Layout.Layout;

namespace Layout.Factory;

public class BoardLayoutFactory : IBoardLayoutFactory
{
    private readonly Dictionary<LayoutType.LayoutType, IBoardLayout> _layouts = new();

    public void RegisterLayout(IBoardLayout layout)
    {
        if (layout == null) throw new ArgumentNullException(nameof(layout));
        _layouts[layout.Type] = layout;
    }

    public IBoardLayout GetRandomLayout(Random? random = null)
    {
        if (_layouts.Count == 0)
            throw new InvalidOperationException("No layouts have been registered");

        var keys = _layouts.Keys.ToList();
        var index = (random ?? new Random()).Next(keys.Count);
        var selectedLayout = keys[index];
        return _layouts[selectedLayout];
    }

    public IBoardLayout GetLayout(LayoutType.LayoutType type)
    {
        if (!_layouts.TryGetValue(type, out var layout))
            throw new InvalidOperationException($"No layout registered for type {type}");
        
        return layout;
    }

    public int GetLayoutCount() => _layouts.Count;

    public IReadOnlyList<LayoutType.LayoutType> GetAvailableLayouts() => _layouts.Keys.ToList();
}