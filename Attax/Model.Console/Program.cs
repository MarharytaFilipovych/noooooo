using Controller;
using Controller.Bot;
using Controller.Commands;
using Controller.Presenters;
using Model.Game;
using View;

namespace Ataxx.Console;

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

        var game = new AtaxxGameWithEvents(DefaultBoardSize);

        IBotStrategy botStrategy = new RandomBotStrategy(random);
        var botOrchestrator = new BotOrchestrator(botStrategy, BotThinkingDelayMs);

        IViewFactory viewFactory = new ViewFactory();
        IViewSwitcher viewSwitcher = new ViewSwitcher(viewFactory, InitialViewType);

        var initialView = viewFactory.CreateView(InitialViewType);
        IGameModeSelector gameModeSelector = new ConsoleGameModeSelector(initialView);

        var presenter = new GamePresenter(game, viewSwitcher);
        var uiController = new GameUIController(viewSwitcher, gameModeSelector);

        var commandProcessor = new CommandProcessor();

        return new GameFlowController(
            game, 
            botOrchestrator, 
            commandProcessor, 
            presenter,
            uiController);
    }
}