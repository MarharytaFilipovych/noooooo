using Model;
using Model.Game.Game;
using Model.PlayerType;
using Stats;
using View.Views;
using ViewSwitcher;

namespace ConsoleOutput;

public class ConsoleOutput(AtaxxGameWithEvents game, IViewSwitcher viewSwitcher) : IConsoleOutput
{
    private IGameView View => viewSwitcher.CurrentView;

    public void ListenTo()
    {
        game.GameStarted += OnGameStarted;
        game.TurnChanged += OnTurnChanged;
        game.MoveMade += OnMoveMade;
        game.MoveInvalid += OnMoveInvalid;
        game.PlayerWon += OnPlayerWon;
        game.GameDrawn += OnGameDrawn;
        game.BoardUpdated += OnBoardUpdated;
        game.HintRequested += OnHintRequested;
        game.ModeSet += OnModeSet;
        game.StatsRequested += OnStatsRequested;
        game.TurnTimedOut += OnTurnTimeOut;
        game.MoveUndone += OnMoveUndo;
        viewSwitcher.ViewSelected += OnViewSelected;
        game.HelpRequested += OnHelpRequested;
        game.ErrorOccurred += OnErrorOccurred;

    }

    private void OnMoveUndo(bool success, PlayerType playerType) =>
        View.DisplayUndo(success, playerType);
    
    private void OnHintRequested(List<Move.Move> moves) => View.DisplayHint(moves);

    private void OnModeSet(GameMode.GameModeType gameModeType) =>
        View.DisplayMessage(gameModeType == GameMode.GameModeType.PvE 
            ? "You are Player X, Bot is Player O" : "ENJOY!");
    
    private void OnGameStarted(Cell[,] board, string layoutName) =>
        View.DisplayGameStart(game.GetGameState(), layoutName, game.GameMode.ModeType);

    private void OnBoardUpdated(Cell[,] board) => View.UpdateBoard(game.GetGameState());

    private void OnTurnChanged(PlayerType player)
    {
        var isBot = game.GameMode.IsBot(player);
        View.DisplayTurn(player, isBot);
    }

    private void OnMoveMade(Move.Move move, PlayerType player)
    {
        var isBot = game.GameMode.IsBot(player);
        View.DisplayMove(move, player, isBot);
    }

    private void OnMoveInvalid(Move.Move move, PlayerType player) => View.DisplayInvalidMove(move);

    private void OnPlayerWon(PlayerType winner) => View.DisplayGameEnd(game.GetGameState(), winner);

    private void OnGameDrawn() => View.DisplayGameEnd( game.GetGameState(), PlayerType.None);

    private void OnViewSelected()
    {
        View.UpdateBoard(game.GetGameState());
        View.DisplayMessage($"Switched to {viewSwitcher.CurrentViewType} view");
    }

    private void OnStatsRequested(GameStatistics statistics) => View.DisplayStatistics(statistics);

    private void OnTurnTimeOut(PlayerType playerType) => View.DisplayElapsedTimeOutMessage(playerType);
    
    private void OnHelpRequested(List<(string Name, string Usage, string Description)> commands) =>
        View.DisplayHelp(commands);
    
    private void OnErrorOccurred(string error) => View.DisplayError(error);
}