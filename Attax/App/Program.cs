using App;
using ConsoleOutput;
using Presenter;

var container = Configuration.ConfigureContainer();

Configuration.ConfigureLayouts(container);
Configuration.ConfigureViews(container);
Configuration.ConfigureGameModes(container);
Configuration.ConfigureBotDifficulties(container);
Configuration.ConfigureBotStrategies(container);
Configuration.ConfigureCommands(container);

var presenter = container.Resolve<IGamePresenter>();
var consoleOutput = container.Resolve<IConsoleOutput>();

consoleOutput.ListenTo();
presenter.Start();
