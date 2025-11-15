using GameMode.Factory;
using Layout.Factory;
using Layout.LayoutType;
using Model.Board;
using Model.Game.Settings;
using View.Views;

namespace Configurator;

public class GameConfigurator(IGameView view, IGameModeFactory gameModeFactory,
    IBoardLayoutFactory layoutFactory, IGameSettings gameSettings) : IGameConfigurator
{
    public void Configure()
    {
        ConfigureMode();
        ConfigureBoardSize();
        ConfigureLayout();
    }

    private void ConfigureMode()
    {
        var modes = gameModeFactory.GetAvailableModes();
        
        if (modes.Count == 0)
            throw new InvalidOperationException("No game modes were registered!");
        
        var optionsForView = modes
            .Select(m => (m.DisplayName, m.Description))
            .ToList();
            
        view.DisplayModeOptions(optionsForView);
        
        var selectedMode = GetModeSelection(modes);
        gameSettings.GameModeType = selectedMode.ModeType;
    }

    private GameModeOption GetModeSelection(IReadOnlyList<GameModeOption> modes)
    {
        var input = view.DisplayGetInput();
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= modes.Count)
            return modes[choice - 1];
        
        view.DisplayError($"Invalid selection. Defaulting to {modes[0].DisplayName}");
        return modes[0];
    }

    private void ConfigureBoardSize()
    {
        view.DisplayMessage($"Select board size ({BoardConstants.MinBoardSize}-{BoardConstants.MaxBoardSize}) [default: {BoardConstants.DefaultSize}]:");
        
        var input = view.DisplayGetInput();
        
        if (string.IsNullOrWhiteSpace(input))
        {
            view.DisplayMessage($"Using default board size: {BoardConstants.DefaultSize}");
            return;
        }
        
        if (int.TryParse(input, out var size) && 
            size is >= BoardConstants.MinBoardSize and <= BoardConstants.MaxBoardSize)
        {
            gameSettings.BoardSize = size;
            view.DisplayMessage($"Board size set to: {size}x{size}");
        }
        else view.DisplayError($"Invalid size. Using default: {BoardConstants.DefaultSize}");
    }

    private void ConfigureLayout()
    {
        var layouts = layoutFactory.GetAvailableLayouts();
        
        view.DisplayMessage("Select board layout:");
        
        for (var i = 0; i < layouts.Count; i++)
        {
            view.DisplayMessage($"{i + 1}. {layouts[i].GetDescription()}");
        }
        
        var input = view.DisplayGetInput();
        
        if (string.IsNullOrWhiteSpace(input) || input == "0")
        {
            view.DisplayMessage("Random layout will be selected");
            return;
        }
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= layouts.Count)
        {
            gameSettings.LayoutType = layouts[choice - 1];
            view.DisplayMessage($"Layout set to: {layouts[choice - 1].GetDescription()}");
        }
        else view.DisplayMessage("Invalid selection. Random layout will be selected");
    }
}
