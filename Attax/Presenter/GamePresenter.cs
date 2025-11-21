using Bot;
using Bot.BotFactory;
using Bot.Orchestrator;
using Commands;
using Commands.CommandProcessor;
using Configurator;
using GameMode.ModeConfigurations;
using Model.Game.Game;
using Model.Game.Settings;
using ViewSwitcher;

namespace Presenter;

public class GamePresenter(
    AtaxxGameWithEvents game,
    IViewSwitcher viewSwitcher,
    IBotOrchestrator botOrchestrator,
    ICommandProcessor commandProcessor,
    IGameSettings gameSettings,
    IGameConfigurator gameConfigurator) : IGamePresenter
{
    public void Start()
    {
        viewSwitcher.CurrentView.DisplayWelcome();
        gameSettings.Reset();

        gameConfigurator.Configure();

        game.StartGame();
        game.SetMode();
        game.StartTimer();

        GameLoop();
    }

    private void GameLoop()
    {
        while (!game.IsEnded)
        {
            if (game.GameMode.IsBot(game.CurrentPlayer))
            {
                HandleBotTurn();
            }
            else ProcessPlayerInput();
        }

        game.EndGame();
    }

    private void HandleBotTurn()
    {
        var config = (PvEConfiguration)game.GameMode;
        var suggestedMove = botOrchestrator.SuggestMove(game, config.BotPlayer);

        if (suggestedMove == null)
        {
            return;
        }

        game.MakeMove(suggestedMove.From, suggestedMove.To);
    }

    private void ProcessPlayerInput()
    {
        var input = viewSwitcher.CurrentView.DisplayGetInput();
        if (string.IsNullOrWhiteSpace(input)) return;

        var args = ParseInput(input);
        var result = commandProcessor.ProcessCommand(args, out var error);

        switch (result)
        {
            case ExecuteResult.Continue:
                game.ResetTimer();
                break;
            case ExecuteResult.Break:
                Environment.Exit(0);
                break;
            case ExecuteResult.Error:
                if (!string.IsNullOrWhiteSpace(error)) viewSwitcher.CurrentView.DisplayError(error);
                game.ResetTimer();
                break;
        }
    }

    private static string[] ParseInput(string input) =>
        input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
}