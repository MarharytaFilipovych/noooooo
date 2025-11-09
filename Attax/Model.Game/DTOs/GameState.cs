namespace Model.Game.DTOs;

public record GameState(
    int BoardSize,
    CellState[,] Cells,
    PlayerType.PlayerType CurrentPlayer,
    int XCount,
    int OCount,
    bool IsEnded,
    PlayerType.PlayerType Winner,
    string LayoutName
);