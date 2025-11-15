using App;
using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using Commands.CommandProcessor;
using ConsoleOutput;
using Layout;
using Layout.Factory;
using Layout.Layout;
using Model;
using Model.Game.CareTaker;
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
using View.Views;


/*var container = Configuration.ConfigureContainer();
Configuration.ConfigureViews(container);
Configuration.ConfigureCommands(container);

var presenter = container.Resolve<IGamePresenter>();
var consoleOutput = container.Resolve<IConsoleOutput>();*/

BoardLayoutFactory.RegisterLayout(new ClassicLayout());
BoardLayoutFactory.RegisterLayout(new CrossLayout());
BoardLayoutFactory.RegisterLayout(new CenterBlockLayout());
var statsTracker = new StatsTracker(new JsonStatisticsRepository());
var moveExecutor = new MoveExecutor();
var moveValidator = new MoveValidator();
var moveGenerator = new RandomMoveGenerator(moveValidator);
var endDetector = new GameEndDetector();
var turnTimer = new TurnTimer();
var game = new AtaxxGameWithEvents(statsTracker, turnTimer, moveValidator, moveExecutor, moveGenerator, endDetector);
var viewFactory = new ViewFactory();
viewFactory.RegisterView(ViewType.Simple, () => new SimpleView());
viewFactory.RegisterView(ViewType.Enhanced, () => new EnhancedView());
var viewSwitcher = new ViewSwitcher.ViewSwitcher(viewFactory);

var commandProcessor = new CommandProcessor();
commandProcessor.Register(new MoveCommandDefinition(), new MoveCommandExecutor(game));
commandProcessor.Register(new SwitchViewCommandDefinition(), new SwitchViewCommandExecutor(viewSwitcher));
commandProcessor.Register(new QuitCommandDefinition(), new QuitCommandExecutor());
commandProcessor.Register(new HintCommandDefinition(), new HintCommandExecutor(game));
commandProcessor.Register(new HelpCommandDefinition(), new HelpCommandExecutor(commandProcessor.Commands().ToList()));
commandProcessor.Register(new StatsCommandDefinition(), new StatsCommandExecutor(game));

var botOrchestrator = new BotOrchestrator(new RandomBotStrategy());
var presenter = new GamePresenter(game, viewSwitcher, botOrchestrator, commandProcessor);
var consoleOutput = new ConsoleOutput.ConsoleOutput(game, viewSwitcher);


consoleOutput.ListenTo();
presenter.Start();