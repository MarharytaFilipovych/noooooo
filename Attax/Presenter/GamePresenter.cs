using Bot;
using Commands;
using Model.Game.Game;
using Model.Game.Mode;
using Model.PlayerType;
using View;
using ViewSwitcher;

namespace Presenter;

public class GamePresenter(AtaxxGameWithEvents game, IViewSwitcher viewSwitcher,
    BotOrchestrator botOrchestrator, CommandProcessor commandProcessor)
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
                var input = View.GetInput();
                if (string.IsNullOrWhiteSpace(input)) continue;
                var result = commandProcessor.ProcessCommand(ParseInput(input), out var error);
                if (result == ExecuteResult.Break)break;
                if (result == ExecuteResult.Error) View.DisplayError(error!);
            }
        }
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
            "2" => GameModeConfiguration.CreatePvE(PlayerType.X),
            _ => GameModeConfiguration.CreatePvP()
        };
        
        game.SetMode();
    }
    
}