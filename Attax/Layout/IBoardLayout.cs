namespace Layout;

public interface IBoardLayout
{
    string Name { get; }
    LayoutType Type { get; }
    bool IsBlocked(int row, int col, int boardSize);
}
