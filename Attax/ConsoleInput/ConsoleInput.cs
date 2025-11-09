using Commands;
using Model.Game.Mode;
using Model.PlayerType;
using View;
using ViewSwitcher;

namespace ConsoleInput;

public class ConsoleInput(IViewSwitcher viewSwitcher, CommandProcessor commandProcessor) : IConsoleInput
{
    private IGameView View => viewSwitcher.CurrentView;
   
    
    public GameModeConfiguration SelectGameMode()
    {
        View.DisplayMessage("Select game mode:");
        View.DisplayMessage("1 - Player vs Player");
        View.DisplayMessage("2 - Player vs Bot");
        
        var input = View.DisplayMessageForAnswer("Enter choice (1 or 2)");

        return input switch
        {
            "1" => GameModeConfiguration.CreatePvP(),
            "2" => CreatePvEConfiguration(),
            _ => GameModeConfiguration.CreatePvP()
        };
    }

    private GameModeConfiguration CreatePvEConfiguration()
    {
        View.DisplayMessage("You are Player X, Bot is Player O");
        return GameModeConfiguration.CreatePvE(PlayerType.X);
    }
    
   
}

