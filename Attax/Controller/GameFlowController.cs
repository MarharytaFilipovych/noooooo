namespace Controller;

using Controller.Bot;
using Controller.Commands;
using Controller.Presenters;
using Model.Game;
using System;

public class GameFlowController
{
    private readonly AtaxxGameWithEvents _game;
    private readonly BotOrchestrator _botOrchestrator;
    private readonly CommandProcessor _commandProcessor;
    private readonly GamePresenter _presenter;
    private readonly GameUIController _uiController;
    
    private GameModeConfiguration _gameModeConfig;

    public GameFlowController(
        AtaxxGameWithEvents game,
        BotOrchestrator botOrchestrator,
        CommandProcessor commandProcessor,
        GamePresenter presenter,
        GameUIController uiController)
    {
        _game = game ?? throw new ArgumentNullException(nameof(game));
        _botOrchestrator = botOrchestrator ?? throw new ArgumentNullException(nameof(botOrchestrator));
        _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
        _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        _uiController = uiController ?? throw new ArgumentNullException(nameof(uiController));
        
        InitializeCommandHandlers();
    }

    public void Start()
    {
        _gameModeConfig = _uiController.SelectGameMode();
        _presenter.SetGameModeConfiguration(_gameModeConfig);
        
        _game.StartGameWithEvents();
        GameLoop();
    }

    private void InitializeCommandHandlers()
    {
        _commandProcessor.RegisterHandler(new QuitCommandHandler());
        _commandProcessor.RegisterHandler(new SwitchViewCommandHandler(_presenter));
        _commandProcessor.RegisterHandler(new MoveCommandHandler(_game, _presenter));
    }

    private void GameLoop()
    {
        while (!_game.IsEnded)
        {
            if (IsCurrentPlayerBot())
            {
                _botOrchestrator.MakeBotMove(_game, _game.CurrentPlayer);
            }
            else
            {
                var input = _uiController.GetPlayerInput();
                
                if (!_commandProcessor.ProcessCommand(input))
                    break;
            }
        }
    }

    private bool IsCurrentPlayerBot() =>
        _gameModeConfig.IsBot(_game.CurrentPlayer);
}