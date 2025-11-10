namespace Model.Board.Layouts;

public abstract class BoardLayoutFactory
{
    private static readonly IBoardLayout[] Layouts =
    [
        new ClassicLayout(),
        new CrossLayout(),
        new CenterBlockLayout()
    ];

    public static IBoardLayout GetRandomLayout(Random? random = null)
    {
        var rng = random ?? new Random();
        var index = rng.Next(Layouts.Length);
        return Layouts[index];
    }

    public static IBoardLayout GetLayout(int index)
    {
        if (index < 0 || index >= Layouts.Length) 
            throw new ArgumentOutOfRangeException(nameof(index));
        return Layouts[index];
    }

    public static int GetLayoutCount() => Layouts.Length;

    public static IBoardLayout[] GetAllLayouts() =>
        (IBoardLayout[])Layouts.Clone();
    
}