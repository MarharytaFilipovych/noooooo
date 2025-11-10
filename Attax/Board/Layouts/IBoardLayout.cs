namespace Model.Board.Layouts;

public interface IBoardLayout
{
    string Name { get; }
    bool IsBlocked(int row, int col, int boardSize);
}
