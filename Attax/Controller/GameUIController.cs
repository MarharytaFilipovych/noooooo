namespace Controller;

using Controller.Presenters;
using Model.Game;
using System;

public class GameUIController
{
    private readonly IViewSwitcher _viewSwitcher;
    private readonly IGameModeSelector _gameModeSelector;

    public GameUIController(
        IViewSwitcher viewSwitcher, 
        IGameModeSelector gameModeSelector)
    {
        _viewSwitcher = viewSwitcher ?? throw new ArgumentNullException(nameof(viewSwitcher));
        _gameModeSelector = gameModeSelector ?? throw new ArgumentNullException(nameof(gameModeSelector));
    }

    public GameModeConfiguration SelectGameMode() => 
        _gameModeSelector.SelectGameMode();

    public string GetPlayerInput() =>
        _viewSwitcher.CurrentView.GetInput("Enter command (move FROM TO / switch / quit)");

    public void DisplayMessage(string message)
    {
        _viewSwitcher.CurrentView.DisplayMessage(message);
    }
}