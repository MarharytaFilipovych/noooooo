namespace Model.Board;

public class CrossLayout : IBoardLayout
{
    public string Name => "Cross";

    public bool IsBlocked(int row, int col, int boardSize)
    {
        var middle = boardSize / 2;
        return col == middle || row == middle;
    }
}
