using Model.Game.Mode;
using Model.PlayerType;
using Model;
using Model.Game.DTOs;
using Stats;

namespace View;

public interface IGameView
{
    void DisplayWelcome();
    void UpdateBoard(GameState state);
    void DisplayGameStart(GameState state, string layoutName, GameMode mode);
    void DisplayTurn(PlayerType player, bool isBot);
    void DisplayMove(Move.Move move, PlayerType player, bool isBot);
    void DisplayInvalidMove(Move.Move move);
    void DisplayGameEnd(GameState state, PlayerType winner);
    void DisplayHint(List<Move.Move> validMoves);
    void DisplayMessage(string message);
    string GetInput();
    void DisplayError(string error);
    string DisplayModeSelection();
    void DisplayStatistics(GameStatistics statistics);
    void DisplayElapsedTimeOutMessage(PlayerType playerType);
    void DisplayUndo(bool success, PlayerType player);
}