using Model.Board;

namespace Model.Game.Settings;

public class GameSettings : IGameSettings
{
    private int _boardSize = BoardConstants.DefaultSize;
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

    public void MarkGameAsStarted() => _isGameStarted = true;

    public void Reset()
    {
        _isGameStarted = false;
        _boardSize = BoardConstants.DefaultSize;
    }
}