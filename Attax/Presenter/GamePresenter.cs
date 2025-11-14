using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using Commands.CommandProcessor;
using Model.Game.Game;
using Model.Game.Mode;
using Model.PlayerType;
using View;
using View.Views;
using ViewSwitcher;

namespace Presenter;

public class GamePresenter(AtaxxGameWithEvents game, IViewSwitcher viewSwitcher,
    BotOrchestrator botOrchestrator, CommandProcessor commandProcessor) : IGamePresenter
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
            else
            {
                var input = View.DisplayGetInput();
                if (string.IsNullOrWhiteSpace(input)) continue;
                var result = commandProcessor.ProcessCommand(ParseInput(input), out var error);
                if (result == ExecuteResult.Break)break;
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
        var input = View.DisplayModeSelection();
        
        game.GameMode = input switch
        {
            "1" => GameModeConfiguration.CreatePvP(),
            "2" => CreatePvEWithUndo(),
            _ => GameModeConfiguration.CreatePvP()
        };
        
        game.SetMode();
    }

    private void SetSize()
    {
        var input = View.DisplayGetInput();
        
    }
        
    private GameModeConfiguration CreatePvEWithUndo()
    {
        var config = GameModeConfiguration.CreatePvE(PlayerType.X);
        commandProcessor.Register(new UndoCommandDefinition(), new UndoCommandExecutor(game));
        return config;
    }
}