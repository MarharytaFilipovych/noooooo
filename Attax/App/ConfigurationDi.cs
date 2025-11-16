using Bot;
using Bot.BotFactory;
using Bot.Evaluation;
using Bot.Orchestrator;
using Bot.Strategy;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using Commands.CommandProcessor;
using Configurator;
using ConsoleOutput;
using Core;
using GameMode.BotDifficultyFactory;
using GameMode.Factory;
using GameMode.ModeConfigurations;
using GameMode.ModeType;
using Layout.Factory;
using Layout.Layout;
using Model;
using Model.Game.CareTaker;
using Model.Game.EndDetector;
using Model.Game.Game;
using Model.Game.Settings;
using Model.Game.TurnTimer;
using Model.PlayerType;
using Move.Executor;
using Move.Generator;
using Move.Validator;
using Presenter;
using Stats;
using Stats.Repository;
using Stats.Tracker;
using TurnTimer;
using View.ViewFactory;
using View.Views;
using ViewSwitcher;

namespace App;

public static class Configuration
{
    public static DiContainer ConfigureContainer()
    {
        var container = new DiContainer();

        container.Register<StatisticsOptions, StatisticsOptions>(Scope.Singleton);
        container.Register<BotOptions, BotOptions>(Scope.Singleton);
        container.Register<TurnTimerOptions, TurnTimerOptions>(Scope.Singleton);

        container.Register<IGameSettings, GameSettings>(Scope.Singleton);
        container.Register<IBoardLayoutFactory, BoardLayoutFactory>(Scope.Singleton);
        container.Register<IGameModeFactory, GameModeFactory>(Scope.Singleton);
        container.Register<IBotDifficultyFactory, BotDifficultyFactory>(Scope.Singleton);

        container.Register<IViewFactory, ViewFactory>(Scope.Singleton);
        container.Register<IViewSwitcher, ViewSwitcher.ViewSwitcher>(Scope.Singleton);
        container.Register<IConsoleOutput, ConsoleOutput.ConsoleOutput>(Scope.Singleton);

        container.Register<IBotStrategyFactory, BotStrategyFactory>(Scope.Singleton);
        container.Register<IBotOrchestrator, BotOrchestrator>(Scope.Singleton);

        container.Register<IMoveExecutor, MoveExecutor>(Scope.Singleton);
        container.Register<IMoveValidator, MoveValidator>(Scope.Singleton);
        container.Register<IMoveGenerator, RandomMoveGenerator>(Scope.Singleton);
        container.Register<IGameEndDetector, GameEndDetector>(Scope.Singleton);
        container.Register<ITurnTimer, Model.Game.TurnTimer.TurnTimer>(Scope.Singleton);
        container.Register<ICareTakerFactory, CareTakerFactory>(Scope.Singleton);

        container.Register<IStatisticsRepository, JsonStatisticsRepository>(Scope.Singleton);
        container.Register<IStatsTracker, StatsTracker>(Scope.Singleton);

        container.Register<AtaxxGameWithEvents, AtaxxGameWithEvents>(Scope.Singleton);

        container.Register<ICommandProcessor, CommandProcessor>(Scope.Singleton);

        container.Register<IGameConfigurator, GameConfigurator>(Scope.Singleton);
        container.Register<IGamePresenter, GamePresenter>(Scope.Singleton);

        return container;
    }

    public static void ConfigureLayouts(DiContainer container)
    {
        var factory = container.Resolve<IBoardLayoutFactory>();
        factory.RegisterLayout(new ClassicLayout());
        factory.RegisterLayout(new CrossLayout());
        factory.RegisterLayout(new CenterBlockLayout());
    }

    public static void ConfigureViews(DiContainer container)
    {
        var factory = container.Resolve<IViewFactory>();
        factory.RegisterView(ViewType.Simple, () => new SimpleView());
        factory.RegisterView(ViewType.Enhanced, () => new EnhancedView());
    }

    public static void ConfigureGameModes(DiContainer container)
    {
        var factory = container.Resolve<IGameModeFactory>();

        factory.RegisterMode(
            new GameModeOption(GameModeType.PvP, "Player vs Player", "Two human players compete"),
            new PvPConfiguration()
        );

        factory.RegisterMode(
            new GameModeOption(GameModeType.PvE, "Player vs Bot", "Play against AI opponent"),
            new PvEConfiguration(PlayerType.X, BotDifficulty.Easy)
        );
    }

    public static void ConfigureBotDifficulties(DiContainer container)
    {
        var factory = container.Resolve<IBotDifficultyFactory>();

        factory.RegisterDifficulty(
            new BotDifficultyOption(
                BotDifficulty.Easy,
                "Easy Bot",
                "Bot makes random moves"
            )
        );

        factory.RegisterDifficulty(
            new BotDifficultyOption(
                BotDifficulty.Hard,
                "Hard Bot",
                "Bot uses greedy strategy to maximize pieces"
            )
        );
    }

    public static void ConfigureBotStrategies(DiContainer container)
    {
        var factory = container.Resolve<IBotStrategyFactory>();
        var moveExecutor = container.Resolve<IMoveExecutor>();
        var moveValidator = container.Resolve<IMoveValidator>();

        factory.RegisterStrategy(
            BotDifficulty.Easy,
            new RandomBotStrategy());

        factory.RegisterStrategy(
            BotDifficulty.Hard,
            new GreedyBotStrategy(new GreedyMoveEvaluator(moveExecutor, moveValidator)));
    }

    public static void ConfigureCommands(DiContainer container)
    {
        var commandProcessor = container.Resolve<ICommandProcessor>();
        var game = container.Resolve<AtaxxGameWithEvents>();
        var viewSwitcher = container.Resolve<IViewSwitcher>();
        var settings = container.Resolve<IGameSettings>();

        commandProcessor.Register(
            new MoveCommandDefinition(),
            new MoveCommandExecutor(game));

        commandProcessor.Register(
            new SwitchViewCommandDefinition(),
            new SwitchViewCommandExecutor(viewSwitcher));

        commandProcessor.Register(
            new QuitCommandDefinition(),
            new QuitCommandExecutor());

        commandProcessor.Register(
            new HintCommandDefinition(),
            new HintCommandExecutor(game));

        commandProcessor.Register(
            new HelpCommandDefinition(),
            new HelpCommandExecutor(game, commandProcessor.Commands().ToList()));

        commandProcessor.Register(
            new StatsCommandDefinition(),
            new StatsCommandExecutor(game));

        commandProcessor.Register(
            new UndoCommandDefinition(),
            new UndoCommandExecutor(game));
    }
}