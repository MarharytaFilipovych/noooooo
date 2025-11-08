namespace Model.PlayerType;

public static class PlayerTypeExtensions
{
    public static PlayerType GetOpponent(this PlayerType player) =>
        player == PlayerType.X ? PlayerType.O : PlayerType.X;
}
