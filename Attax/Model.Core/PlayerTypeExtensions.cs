namespace Model;

public static class PlayerTypeExtensions
{
    public static PlayerType GetOpponent(this PlayerType player)
    {
        return player == PlayerType.X ? PlayerType.O : PlayerType.X;
    }
}
