using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using Model;
using Model.Game.Game;
using Presenter;
using View;
using View.ViewFactory;


//var container = Configuration.ConfigureContainer();
//Configuration.ConfigureViews(container);
//Configuration.ConfigureCommands(container);

//var presenter = container.Resolve<IGamePresenter>();
//var consoleOutput = container.Resolve<IConsoleOutput>();


var game = new AtaxxGameWithEvents();
var viewFactory = new ViewFactory();
viewFactory.RegisterView(ViewType.Simple, () => new SimpleView());
viewFactory.RegisterView(ViewType.Enhanced, () => new EnhancedView());

var viewSwitcher = new ViewSwitcher.ViewSwitcher(viewFactory);

var commandProcessor = new CommandProcessor();
commandProcessor.Register(new MoveCommandDefinition(), new MoveCommandExecutor(game));
commandProcessor.Register(new SwitchViewCommandDefinition(), new SwitchViewCommandExecutor(viewSwitcher));
commandProcessor.Register(new QuitCommandDefinition(), new QuitCommandExecutor());
commandProcessor.Register(new HintCommandDefinition(), new HintCommandExecutor(game));
commandProcessor.Register(new HelpCommandDefinition(), new HelpCommandExecutor(commandProcessor.Commands.ToList()));

var botOrchestrator = new BotOrchestrator(new RandomBotStrategy());
var presenter = new GamePresenter(game, viewSwitcher, botOrchestrator, commandProcessor);
var consoleOutput = new ConsoleOutput.ConsoleOutput(game, viewSwitcher);


consoleOutput.ListenTo();
presenter.Start();