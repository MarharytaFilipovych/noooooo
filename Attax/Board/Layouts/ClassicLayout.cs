namespace Model.Board.Layouts;

public class ClassicLayout : IBoardLayout
{
    public string Name => "Classic";

    public bool IsBlocked(int row, int col, int boardSize) => false;
}
