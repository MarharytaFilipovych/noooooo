using GameMode.Factory;
using Layout.Factory;
using Layout.LayoutType;
using Model.Board;
using Model.Game.Settings;
using View.Views;
using ViewSwitcher;

namespace Configurator;

public class GameConfigurator(IViewSwitcher viewSwitcher, IGameModeFactory gameModeFactory,
    IBoardLayoutFactory layoutFactory, IGameSettings gameSettings) : IGameConfigurator
{
    private IGameView View => viewSwitcher.CurrentView;
    
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
            
        View.DisplayModeOptions(optionsForView);
        
        var selectedMode = GetModeSelection(modes);
        gameSettings.GameModeType = selectedMode.ModeType;
    }

    private GameModeOption GetModeSelection(IReadOnlyList<GameModeOption> modes)
    {
        var input = View.DisplayGetInput();
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= modes.Count)
            return modes[choice - 1];
        
        View.DisplayError($"Invalid selection. Defaulting to {modes[0].DisplayName}");
        return modes[0];
    }

    private void ConfigureBoardSize()
    {
        View.DisplayMessage($"Select board size ({BoardConstants.MinBoardSize}-{BoardConstants.MaxBoardSize}) [default: {BoardConstants.DefaultSize}]:");
        
        var input = View.DisplayGetInput();
        
        if (string.IsNullOrWhiteSpace(input))
        {
            View.DisplayMessage($"Using default board size: {BoardConstants.DefaultSize}");
            return;
        }
        
        if (int.TryParse(input, out var size) && 
            size is >= BoardConstants.MinBoardSize and <= BoardConstants.MaxBoardSize)
        {
            gameSettings.BoardSize = size;
            View.DisplayMessage($"Board size set to: {size}x{size}");
        }
        else View.DisplayError($"Invalid size. Using default: {BoardConstants.DefaultSize}");
    }

    private void ConfigureLayout()
    {
        var layouts = layoutFactory.GetAvailableLayouts();
        
        View.DisplayMessage("Select board layout:");
        
        for (var i = 0; i < layouts.Count; i++)
        {
            View.DisplayMessage($"{i + 1}. {layouts[i].GetDescription()}");
        }
        
        var input = View.DisplayGetInput();
        
        if (string.IsNullOrWhiteSpace(input) || input == "0")
        {
            View.DisplayMessage("Random layout will be selected");
            return;
        }
        
        if (int.TryParse(input, out var choice) && choice > 0 && choice <= layouts.Count)
        {
            gameSettings.LayoutType = layouts[choice - 1];
            View.DisplayMessage($"Layout set to: {layouts[choice - 1].GetDescription()}");
        }
        else View.DisplayMessage("Invalid selection. Random layout will be selected");
    }
}
