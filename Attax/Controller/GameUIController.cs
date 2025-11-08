using Attax.Presenters;
using Model.Game;

namespace Attax;

public class GameUiController(IViewSwitcher viewSwitcher, IGameModeSelector gameModeSelector)
{
    private readonly IViewSwitcher _viewSwitcher = viewSwitcher ?? throw new ArgumentNullException(nameof(viewSwitcher));
    private readonly IGameModeSelector _gameModeSelector = gameModeSelector ?? throw new ArgumentNullException(nameof(gameModeSelector));

    public GameModeConfiguration SelectGameMode() => 
        _gameModeSelector.SelectGameMode();

    public string GetPlayerInput() =>
        _viewSwitcher.CurrentView.GetInput("Enter command (move FROM TO / switch / quit)");

    public void DisplayMessage(string message) => 
        _viewSwitcher.CurrentView.DisplayMessage(message);
}