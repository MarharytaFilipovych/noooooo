using Bot.Orchestrator;
using Commands;
using Commands.CommandProcessor;
using Model.Game.Game;
using Model.Game.Settings;
using GameMode.Factory;
using View.Views;
using ViewSwitcher;

namespace Presenter;

public class GamePresenter(
    AtaxxGameWithEvents game, 
    IViewSwitcher viewSwitcher,
    IBotOrchestrator botOrchestrator, 
    ICommandProcessor commandProcessor,
    IGameModeFactory gameModeFactory,
    IGameSettings gameSettings) : IGamePresenter  
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
            if (IsCurrentPlayerBot()) botOrchestrator.MakeBotMove(game, game.CurrentPlayer);
            else ProcessPlayerInput();
        }
        
        game.EndGame();
    }

    private void ProcessPlayerInput()
    {
        var input = View.DisplayGetInput();
        if (string.IsNullOrWhiteSpace(input)) return;
        
        var result = commandProcessor.ProcessCommand(ParseInput(input), out var error);
        
        switch (result)
        {
            case ExecuteResult.Break:
                Environment.Exit(0);
                break;
            case ExecuteResult.Error:
                View.DisplayError(error!);
                break;
        }
    }

    private bool IsCurrentPlayerBot() => game.GameMode.IsBot(game.CurrentPlayer);
    
    private static string[] ParseInput(string input) =>                     
        input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

    private void SetMode()
    {
        var modes = gameModeFactory.GetAvailableModes();
        
        if (modes.Count == 0)
            throw new InvalidOperationException("No game modes were registered!");
        
        DisplayModeOptions(modes);
        
        var selectedMode = GetModeSelection(modes);
        
        gameSettings.GameModeType = selectedMode.ModeType;
        
        game.SetMode();
    }

    private void DisplayModeOptions(IReadOnlyList<GameModeOption> modes)
    {
        View.DisplayMessage("Select game mode:");
        for (var i = 0; i < modes.Count; i++)
        {
            View.DisplayMessage($"{i + 1}. {modes[i].DisplayName} - {modes[i].Description}");
        }
    }

    private GameModeOption GetModeSelection(IReadOnlyList<GameModeOption> modes)
    {
        var input = View.DisplayGetInput();
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= modes.Count)
            return modes[choice - 1];
        
        View.DisplayMessage($"Invalid selection. Defaulting to {modes[0].DisplayName}");
        return modes[0];
    }
}