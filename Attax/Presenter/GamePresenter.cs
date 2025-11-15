using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using Commands.CommandProcessor;
using Model.Game.Game;
using Model.Game.Mode;
using View.Views;
using ViewSwitcher;

namespace Presenter;

public class GamePresenter(AtaxxGameWithEvents game, IViewSwitcher viewSwitcher,
    BotOrchestrator botOrchestrator, CommandProcessor commandProcessor,
    IGameModeFactory gameModeFactory) : IGamePresenter
{
    private IGameView View => viewSwitcher.CurrentView;

    public void Start()
    {
        View.DisplayWelcome();
        SetMode();
        game.StartGame();
        GameLoop();
    }
    
    private void GameLoop()
    {
        while (!game.IsEnded)
        {
            if (IsCurrentPlayerBot()) 
                botOrchestrator.MakeBotMove(game, game.CurrentPlayer);
            else
            {
                var input = View.DisplayGetInput();
                if (string.IsNullOrWhiteSpace(input)) continue;
                
                var result = commandProcessor.ProcessCommand(ParseInput(input), out var error);
                if (result == ExecuteResult.Break) break;
                if (result == ExecuteResult.Error) View.DisplayError(error!);
            }
        }
        game.EndGame();
    }

    private bool IsCurrentPlayerBot() => game.GameMode.IsBot(game.CurrentPlayer);
    
    private static string[] ParseInput(string input) =>                     
        input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private void SetMode()
    {
        var modes = gameModeFactory.GetAvailableModes();
        
        View.DisplayMessage("Select game mode:");
        for (var i = 0; i < modes.Count; i++)
        {
            View.DisplayMessage($"{i + 1}. {modes[i].DisplayName} - {modes[i].Description}");
        }
        
        var input = View.DisplayGetInput();
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= modes.Count)
        {
            var selectedMode = modes[choice - 1];
            game.GameMode = gameModeFactory.CreateMode(selectedMode.Key);
            
            if (selectedMode.Key == "pve")
                commandProcessor.Register(new UndoCommandDefinition(), new UndoCommandExecutor(game));
        }
        else
        {
            var defaultMode = modes[0];
            game.GameMode = gameModeFactory.CreateMode(defaultMode.Key);
            View.DisplayMessage($"Invalid selection. Defaulting to {defaultMode.DisplayName}");
        }
        
        game.SetMode();
    }
}