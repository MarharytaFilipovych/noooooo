namespace Layout;

public static class BoardLayoutFactory 
{
    private static readonly Dictionary<LayoutType, IBoardLayout> Layouts = new();

    public static void RegisterLayout(IBoardLayout layout) =>
        Layouts[layout.Type] = layout ?? throw new ArgumentNullException(nameof(layout));

    public static IBoardLayout GetRandomLayout(Random? random = null)
    {
        if (Layouts.Count == 0)
            throw new InvalidOperationException("No layouts have been registered.");

        var keys = Layouts.Keys.ToList();
        var index = (random ?? new Random()).Next(keys.Count);
        var selectedLayout = keys[index];
        return Layouts[selectedLayout];
    }

    public static IBoardLayout GetLayout(LayoutType type) =>
        Layouts.TryGetValue(type, out var layout)
            ? layout
            : throw new InvalidOperationException($"No layout registered for type {type}.");

    public static int GetLayoutCount() => Layouts.Count;

    public static IReadOnlyList<LayoutType> GetAvailableLayouts() =>
        Layouts.Keys.ToList();
}