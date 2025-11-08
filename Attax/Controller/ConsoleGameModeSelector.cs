using Attax;
using Model.Game.Mode;
using Model.PlayerType;

namespace Controller;

using Model;
using Model.Game;
using View;
using System;

public class ConsoleGameModeSelector : IGameModeSelector
{
    private readonly IGameView _view;

    public ConsoleGameModeSelector(IGameView view)
    {
        _view = view ?? throw new ArgumentNullException(nameof(view));
    }

    public GameModeConfiguration SelectGameMode()
    {
        _view.DisplayMessage("Select game mode:");
        _view.DisplayMessage("1 - Player vs Player");
        _view.DisplayMessage("2 - Player vs Bot");
        
        var input = _view.GetInput("Enter choice (1 or 2)");
        
        return input switch
        {
            "1" => GameModeConfiguration.CreatePvP(),
            "2" => CreatePvEConfiguration(),
            _ => GameModeConfiguration.CreatePvP()
        };
    }

    private GameModeConfiguration CreatePvEConfiguration()
    {
        _view.DisplayMessage("You are Player X, Bot is Player O");
        return GameModeConfiguration.CreatePvE(PlayerType.X);
    }
}