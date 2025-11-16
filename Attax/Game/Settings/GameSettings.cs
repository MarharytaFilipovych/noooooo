using GameMode;
using GameMode.ModeConfigurations;
using GameMode.ModeType;
using Layout.LayoutType;
using Model.Board;

namespace Model.Game.Settings;

public class GameSettings : IGameSettings
{
    private int _boardSize = BoardConstants.DefaultSize;
    private LayoutType? _layoutType;
    private GameModeType? _gameModeType;
    private BotDifficulty? _botDifficulty;
    private bool _isGameStarted;
    
    public int BoardSize
    {
        get => _boardSize;
        set
        {
            if (_isGameStarted)
                throw new InvalidOperationException("You can't change the board size after game has started!!!");
            
            if (value is < BoardConstants.MinBoardSize or > BoardConstants.MaxBoardSize)
                throw new ArgumentOutOfRangeException(nameof(value), 
                    $"Board size must be between {BoardConstants.MinBoardSize} " +
                    $"and {BoardConstants.MaxBoardSize}");
            
            _boardSize = value;
        }
    }

    public LayoutType? LayoutType
    {
        get => _layoutType;
        set
        {
            if (_isGameStarted)
                throw new InvalidOperationException("You can't change the layout after game has started!!!");
            
            _layoutType = value;
        }
    }
    
    public GameModeType? GameModeType
    {
        get => _gameModeType;
        set
        {
            if (_isGameStarted)
                throw new InvalidOperationException("You can't change the game mode after game has started!!!");
            
            _gameModeType = value;
        }
    }

    public BotDifficulty? BotDifficulty
    {
        get => _botDifficulty;
        set
        {
            if (_isGameStarted)
                throw new InvalidOperationException("You can't change the bot difficulty after game has started!!!");
            
            _botDifficulty = value;
        }
    }

    public void MarkGameAsStarted() => _isGameStarted = true;

    public void Reset()
    {
        _isGameStarted = false;
        _boardSize = BoardConstants.DefaultSize;
        _layoutType = null;
        _gameModeType = null;
        _botDifficulty = null;
    }
}


