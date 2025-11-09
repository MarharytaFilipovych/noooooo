using Model.Game.Mode;
using Model.PlayerType;
using View;

namespace ModeSelector;


public class ConsoleModeSelector(IGameView view) : IModeSelector
{

    public GameModeConfiguration SelectGameMode()
    {
        view.DisplayMessage("Select game mode:");
        view.DisplayMessage("1 - Player vs Player");
        view.DisplayMessage("2 - Player vs Bot");
        
        var input = view.DisplayMessageForAnswer("Enter choice (1 or 2)");
        
        return input switch
        {
            "1" => GameModeConfiguration.CreatePvP(),
            "2" => CreatePvEConfiguration(),
            _ => GameModeConfiguration.CreatePvP()
        };
    }

    private GameModeConfiguration CreatePvEConfiguration()
    {
        view.DisplayMessage("You are Player X, Bot is Player O");
        return GameModeConfiguration.CreatePvE(PlayerType.X);
    }
}