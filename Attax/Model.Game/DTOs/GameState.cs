namespace Model.Game.DTOs;

public record GameState(
    int BoardSize,
    CellState[,] Cells,
    PlayerType CurrentPlayer,
    int XCount,
    int OCount,
    bool IsEnded,
    PlayerType Winner,
    string LayoutName
);