using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using ConsoleOutput;
using Core;
using Model;
using Model.Board.Layouts;
using Model.Game.EndDetector;
using Model.Game.Game;
using Model.Game.TurnTimer;
using Move.Executor;
using Move.Generator;
using Move.Validator;
using Presenter;
using Stats.Repository;
using Stats.Tracker;
using View;
using View.ViewFactory;
using ViewSwitcher;

namespace App;

public static class Configuration
{
    public static DiContainer ConfigureContainer()
    {
        var container = new DiContainer();

        container.Register<IViewFactory, ViewFactory>(Scope.Singleton);
        container.Register<IViewSwitcher, ViewSwitcher.ViewSwitcher>(Scope.Singleton);
        container.Register<IConsoleOutput, ConsoleOutput.ConsoleOutput>(Scope.Singleton);
        container.Register<IBotStrategy, RandomBotStrategy>(Scope.Singleton);

        container.Register<IMoveExecutor, MoveExecutor>(Scope.Singleton);
        container.Register<IMoveValidator, MoveValidator>(Scope.Singleton);
        container.Register<IMoveGenerator, RandomMoveGenerator>(Scope.Singleton);
        container.Register<IGameEndDetector, GameEndDetector>(Scope.Singleton);
        container.Register<ITurnTimer, TurnTimer>(Scope.Singleton);
        container.Register<IStatisticsRepository, JsonStatisticsRepository>(Scope.Singleton);
        container.Register<IStatsTracker, StatsTracker>(Scope.Singleton);
        container.Register<IBoardLayout, ClassicLayout>(Scope.Singleton);

        var statsTracker = container.Resolve<IStatsTracker>();
        var turnTimer = container.Resolve<ITurnTimer>();
        var moveValidator = container.Resolve<IMoveValidator>();
        var moveExecutor = container.Resolve<IMoveExecutor>();
        var moveGenerator = container.Resolve<IMoveGenerator>();
        var boardLayout = container.Resolve<IBoardLayout>();
        var endDetector = container.Resolve<IGameEndDetector>();

        var game = new AtaxxGameWithEvents(statsTracker, turnTimer, moveValidator,
            moveExecutor, moveGenerator, endDetector, boardLayout);

        container.RegisterInstance(game);
        
        //container.Register<AtaxxGameWithEvents, AtaxxGameWithEvents>(Scope.Singleton);

        container.Register<ICommandProcessor, CommandProcessor>(Scope.Singleton);
        container.Register<IBotOrchestrator, BotOrchestrator>(Scope.Singleton);
        container.Register<IGamePresenter, GamePresenter>(Scope.Singleton);

        return container;
    }

    public static void ConfigureViews(DiContainer container)
    {
        var factory = container.Resolve<IViewFactory>();
        factory.RegisterView(ViewType.Simple, () => new SimpleView());
        factory.RegisterView(ViewType.Enhanced, () => new EnhancedView());
    }

    public static void ConfigureCommands(DiContainer container)
    {
        var game = container.Resolve<AtaxxGameWithEvents>();
        var commandProcessor = container.Resolve<CommandProcessor>();
        var viewSwitcher = container.Resolve<IViewSwitcher>();

        commandProcessor.Register(new MoveCommandDefinition(), new MoveCommandExecutor(game));
        commandProcessor.Register(new SwitchViewCommandDefinition(), new SwitchViewCommandExecutor(viewSwitcher));
        commandProcessor.Register(new QuitCommandDefinition(), new QuitCommandExecutor());
        commandProcessor.Register(new HintCommandDefinition(), new HintCommandExecutor(game));
        commandProcessor.Register(new HelpCommandDefinition(), new HelpCommandExecutor(commandProcessor.Commands().ToList()));
        commandProcessor.Register(new StatsCommandDefinition(), new StatsCommandExecutor(game));
    }
}
