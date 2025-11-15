namespace Layout.Layout;

public interface IBoardLayout
{
    string Name { get; }
    LayoutType.LayoutType Type { get; }
    bool IsBlocked(int row, int col, int boardSize);
}
