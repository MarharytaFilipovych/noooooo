using Model;
using Model.Game;
using Model.Game.Game;
using Model.Game.Mode;
using Model.PlayerType;

namespace Attax.Presenters;

public class GamePresenter
{
    private readonly AtaxxGameWithEvents _game;
    private readonly IViewSwitcher _viewSwitcher;
    
    private GameModeConfiguration? _gameModeConfig;

    public GamePresenter(AtaxxGameWithEvents game, IViewSwitcher viewSwitcher)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _viewSwitcher = viewSwitcher ?? throw new ArgumentNullException(nameof(viewSwitcher));
        
        SubscribeToGameEvents();
    }

    public void SetGameModeConfiguration(GameModeConfiguration config) =>
        _gameModeConfig = config ?? throw new ArgumentNullException(nameof(config));

    private void SubscribeToGameEvents()
    {
        _game.GameStarted += OnGameStarted;
        _game.TurnChanged += OnTurnChanged;
        _game.MoveMade += OnMoveMade;
        _game.MoveInvalid += OnMoveInvalid;
        _game.PlayerWon += OnPlayerWon;
        _game.GameDrawn += OnGameDrawn;
        _game.BoardUpdated += OnBoardUpdated;
    }

    private void OnGameStarted(Cell[,] board, string layoutName)
    {
        var state = _game.GetGameState();
        _viewSwitcher.CurrentView.DisplayGameStart(state, layoutName, _gameModeConfig!.Mode);
    }

    private void OnBoardUpdated(Cell[,] board)
    {
        var state = _game.GetGameState();
        _viewSwitcher.CurrentView.UpdateBoard(state);
    }

    private void OnTurnChanged(PlayerType player)
    {
        var isBot = _gameModeConfig?.IsBot(player) ?? false;
        _viewSwitcher.CurrentView.DisplayTurn(player, isBot);
    }

    private void OnMoveMade(Move move, PlayerType player)
    {
        var isBot = _gameModeConfig?.IsBot(player) ?? false;
        _viewSwitcher.CurrentView.DisplayMove(move, player, isBot);
    }

    private void OnMoveInvalid(Move move, PlayerType player) => _viewSwitcher.CurrentView.DisplayInvalidMove(move);

    private void OnPlayerWon(PlayerType winner)
    {
        var state = _game.GetGameState();
        _viewSwitcher.CurrentView.DisplayGameEnd(state, winner);
    }

    private void OnGameDrawn()
    {
        var state = _game.GetGameState();
        _viewSwitcher.CurrentView.DisplayGameEnd(state, PlayerType.None);
    }

    public void SwitchView()
    {
        _viewSwitcher.SwitchView();
        
        var state = _game.GetGameState();
        _viewSwitcher.CurrentView.UpdateBoard(state);
        _viewSwitcher.CurrentView.DisplayMessage($"Switched to {_viewSwitcher.CurrentViewType} view");
    }

    public void DisplayMessage(string message) => _viewSwitcher.CurrentView.DisplayMessage(message);
}