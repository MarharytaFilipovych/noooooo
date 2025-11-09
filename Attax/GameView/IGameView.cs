using Model.Game.Mode;
using Model.PlayerType;
using Model;
using Model.Game.DTOs;

namespace View;

public interface IGameView
{
    void DisplayWelcome();
    void UpdateBoard(GameState state);
    void DisplayGameStart(GameState state, string layoutName, GameMode mode);
    void DisplayTurn(PlayerType player, bool isBot);
    void DisplayMove(Move move, PlayerType player, bool isBot);
    void DisplayInvalidMove(Move move);
    void DisplayGameEnd(GameState state, PlayerType winner);
    void DisplayHint(List<Move> validMoves);
    void DisplayMessage(string message);
    string GetInput();
    string DisplayMessageForAnswer(string message);
    void DisplayError(string error);
    string DisplayModeSelection();
}