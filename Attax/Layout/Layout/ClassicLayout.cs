namespace Layout.Layout;

public class ClassicLayout : IBoardLayout
{
    public string Name => "Classic";
    public LayoutType Type => LayoutType.Classic;

    public bool IsBlocked(int row, int col, int boardSize) => false;
}
