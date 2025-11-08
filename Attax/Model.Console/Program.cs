using Attax;
using Attax.Bot;
using Attax.Commands;
using Attax.Presenters;
using Controller;
using Model.Game.Game;
using View;
using View.ViewFactory;

namespace Model.Console;

public static class Program
{
    private const int DefaultBoardSize = 7;
    private const int BotThinkingDelayMs = 500;
    private const ViewType InitialViewType = ViewType.Simple;

    public static void Main(string[] args)
    {
        try
        {
            var application = CreateApplication();
            application.Start();
        }
        catch (Exception ex)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Error: {ex.Message}");
            System.Console.ResetColor();
        }
    }

    private static GameFlowController CreateApplication()
    {
        var random = new Random();

        var game = new AtaxxGameWithEvents();

        IBotStrategy botStrategy = new RandomBotStrategy(random);
        var botOrchestrator = new BotOrchestrator(botStrategy);

        var viewFactory = new ViewFactory();
        var viewSwitcher = new ViewSwitcher(viewFactory);

        var initialView = viewFactory.CreateView(InitialViewType);
        IGameModeSelector gameModeSelector = new ConsoleGameModeSelector(initialView);

        var presenter = new GamePresenter(game, viewSwitcher);
        var uiController = new GameUiController(viewSwitcher, gameModeSelector);

        var commandProcessor = new CommandProcessor();

        return new GameFlowController(
            game, 
            botOrchestrator, 
            commandProcessor, 
            presenter,
            uiController, null);
    }
}