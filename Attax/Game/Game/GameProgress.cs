using Model.PlayerType;

namespace Model.Game.Game;


public class GameProgress 
{
    public PlayerType.PlayerType CurrentPlayer { get; private set; }
    public PlayerType.PlayerType Winner { get; private set; }
    public bool IsEnded { get; private set; }
    public int TurnNumber { get; private set; }

    public void Initialize()
    {
        CurrentPlayer = PlayerType.PlayerType.X;
        Winner = PlayerType.PlayerType.None;
        IsEnded = false;
        TurnNumber = 1;
    }

    public void AdvanceTurn()
    {
        CurrentPlayer = CurrentPlayer.GetOpponent();
        TurnNumber++;
    }

    public void EndGame(PlayerType.PlayerType winner)
    {
        IsEnded = true;
        Winner = winner;
    }

    public void Restore(PlayerType.PlayerType player, int turn)
    {
        CurrentPlayer = player;
        TurnNumber = turn;
        IsEnded = false;
        Winner = PlayerType.PlayerType.None;
    }
}