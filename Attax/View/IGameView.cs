using Model.Game.Mode;
using Model.PlayerType;

namespace View;

using Model;
using Model.Game;
using Model.Game.DTOs;

public interface IGameView
{
    void UpdateBoard(GameState state);
    void DisplayGameStart(GameState state, string layoutName, GameMode mode);
    void DisplayTurn(PlayerType player, bool isBot);
    void DisplayMove(Move move, PlayerType player, bool isBot);
    void DisplayInvalidMove(Move move);
    void DisplayGameEnd(GameState state, PlayerType winner);
    
    void DisplayMessage(string message)
    {
        Console.WriteLine($" {message}");
    }

    string GetInput(string prompt)
    {
        Console.Write($"> {prompt}: ");
        return Console.ReadLine() ?? string.Empty;
    }
}