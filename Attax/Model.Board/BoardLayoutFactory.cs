namespace Model.Board;

using System;

public class BoardLayoutFactory
{
    private static readonly IBoardLayout[] layouts = new IBoardLayout[]
    {
        new ClassicLayout(),
        new CrossLayout(),
        new CenterBlockLayout()
    };

    public static IBoardLayout GetRandomLayout(Random random = null)
    {
        var rng = random ?? new Random();
        int index = rng.Next(layouts.Length);
        return layouts[index];
    }

    public static IBoardLayout GetLayout(int index)
    {
        if (index < 0 || index >= layouts.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        return layouts[index];
    }

    public static int GetLayoutCount()
    {
        return layouts.Length;
    }

    public static IBoardLayout[] GetAllLayouts()
    {
        return (IBoardLayout[])layouts.Clone();
    }
}