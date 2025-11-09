using Bot;
using Commands;
using Commands.CommandDefinition;
using Commands.CommandExecutor;
using ConsoleOutput;
using Core;
using Model;
using Model.Game.Game;
using Presenter;
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
        container.Register<IBotStrategy, RandomBotStrategy>(Scope.Singleton);
        container.Register<IGamePresenter, GamePresenter>(Scope.Singleton);
        container.Register<IConsoleOutput, ConsoleOutput.ConsoleOutput>(Scope.Singleton);
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
        var game = new AtaxxGameWithEvents();
        var context = new CommandProcessor();
        var viewSwitcher = container.Resolve<IViewSwitcher>();

        context.Register(new MoveCommandDefinition(), new MoveCommandExecutor(game));
        context.Register(new SwitchViewCommandDefinition(), new SwitchViewCommandExecutor(viewSwitcher));
        context.Register(new QuitCommandDefinition(), new QuitCommandExecutor());
        context.Register(new HintCommandDefinition(), new HintCommandExecutor(game));
        context.Register(new HelpCommandDefinition(), new HelpCommandExecutor(context.Commands.ToList()));
    }
}