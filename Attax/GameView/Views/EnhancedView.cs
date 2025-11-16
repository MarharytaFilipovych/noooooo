using Model.Game.DTOs;
using Model.PlayerType;
using Stats;

namespace View.Views;

public class EnhancedView : IGameView
{
    public void DisplayWelcome()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════╗");
        Console.WriteLine("║      ⚔️  WELCOME TO ATAXX GAME ⚔️      ║");
        Console.WriteLine("║     Type 'help' for available commands  ║");
        Console.WriteLine("╚══════════════════════════════════╝\n");
    }


    public void UpdateBoard(GameState state)
    {
        Console.WriteLine("\nAtaxx - Enhanced GameView❤️❤️❤️❤️");
        Console.WriteLine("────────────────────────────────");

        PrintHeader(state.BoardSize);
        PrintTopBorder(state.BoardSize);

        for (var row = 0; row < state.BoardSize; row++)
        {
            PrintRow(row, state.Cells);
            if (row < state.BoardSize - 1)
                PrintMiddleBorder(state.BoardSize);
        }

        PrintBottomBorder(state.BoardSize);

        Console.WriteLine($"\nScore │ X: {state.XCount} │ O: {state.OCount}");
    }

    private void PrintHeader(int boardSize)
    {
        Console.Write("   ");
        for (var col = 0; col < boardSize; col++)
        {
            Console.Write($" {(char)('A' + col)} ");
            if (col < boardSize - 1) Console.Write(" ");
        }

        Console.WriteLine();
    }

    private void PrintTopBorder(int boardSize)
    {
        Console.Write("  ┌");
        for (var col = 0; col < boardSize; col++)
        {
            Console.Write("───");
            if (col < boardSize - 1) Console.Write("┬");
        }

        Console.WriteLine("┐");
    }

    private void PrintMiddleBorder(int boardSize)
    {
        Console.Write("  ├");
        for (var col = 0; col < boardSize; col++)
        {
            Console.Write("───");
            if (col < boardSize - 1) Console.Write("┼");
        }

        Console.WriteLine("┤");
    }

    private void PrintBottomBorder(int boardSize)
    {
        Console.Write("  └");
        for (var col = 0; col < boardSize; col++)
        {
            Console.Write("───");
            if (col < boardSize - 1) Console.Write("┴");
        }

        Console.WriteLine("┘");
    }

    private void PrintRow(int rowIndex, CellState[,] cells)
    {
        var boardSize = cells.GetLength(0);
        Console.Write($"{rowIndex + 1} │");
        for (var col = 0; col < boardSize; col++)
        {
            var symbol = cells[rowIndex, col].ToSymbol();
            Console.Write($" {symbol} ");
            if (col < boardSize - 1) Console.Write("│");
        }

        Console.WriteLine("│");
    }

    public void DisplayGameStart(GameState state, string layoutName, string mode)
    {
        Console.Clear();
        Console.WriteLine("════════════════════════════════");
        Console.WriteLine("    Game started");
        Console.WriteLine($"    LayoutType: {layoutName}");
        Console.WriteLine($"    ModeType: {mode}");
        Console.WriteLine("════════════════════════════════");
        UpdateBoard(state);
    }

    public void DisplayTurn(PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"\n> {playerLabel}'s turn");
    }

    public void DisplayMove(Move.Move move, PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($" {playerLabel} moved: {move}");
    }

    public void DisplayInvalidMove(Move.Move move) => Console.WriteLine($" Invalid move: {move}");

    public void DisplayGameEnd(GameState state, PlayerType winner)
    {
        UpdateBoard(state);

        Console.WriteLine("\n════════════════════════════════");
        Console.WriteLine(winner == PlayerType.None
            ? "    Game ended in a draw!"
            : $"    Player {winner} wins!");


        Console.WriteLine("════════════════════════════════");
    }

    public void DisplayHint(List<Move.Move> validMoves)
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║       Valid Moves            ║");
        Console.WriteLine("╚══════════════════════════════╝");
        validMoves.ForEach(move => Console.WriteLine($"  → {move}"));
    }

    public void DisplayMessage(string message) =>
        Console.WriteLine($"❤️❤️❤️❤️❤️❤️ {message}");

    public string DisplayGetInput()
    {
        Console.Write("❤️ ");
        return Console.ReadLine() ?? string.Empty;
    }

    public void DisplayError(string error)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"OHHHHH NOOOOOOOOOOO🥺🥺🥺🥺🥺: {error}");
        Console.ResetColor();
    }

    public void DisplayStatistics(GameStatistics stats)
    {
        const int valueWidth = 8;

        Console.WriteLine("\n╔════════════════════════════════════════╗");
        Console.WriteLine("║           🎮 Game Statistics 🎮          ║");
        Console.WriteLine("╠════════════════════════════════════════╣");

        Console.WriteLine($"║ 🕹 Total Games:     {stats.GamesPlayed,valueWidth}             ║");
        Console.WriteLine($"║ ❌ Player X Wins:   {stats.PlayerXWins,valueWidth}             ║");
        Console.WriteLine($"║ ⭘ Player O Wins:   {stats.PlayerOWins,valueWidth}             ║");
        Console.WriteLine($"║ ⚖ Draws:           {stats.Draws,valueWidth}             ║");
        Console.WriteLine($"║ 📊 Avg Moves:       {stats.AverageMoveCount,valueWidth:F1}           ║");
        Console.WriteLine($"║ 🗓 Last Played:     {stats.LastPlayed:yyyy-MM-dd}         ║");

        Console.WriteLine("╚════════════════════════════════════════╝\n");
    }

    public void DisplayElapsedTimeOutMessage(PlayerType playerType)
    {
        Console.WriteLine("\n╔════════════════════════════════════════════╗");
        Console.WriteLine("║          ⏰ Turn Time Expired! ⏰          ║");
        Console.WriteLine($"║  Player {playerType} did not move in time! ║");
        Console.WriteLine("║  A random move has been applied automatically. ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
    }

    public void DisplayUndo(bool success, PlayerType player)
    {
        Console.WriteLine("\n╔════════════════════════════════════════╗");
        if (success)
        {
            Console.WriteLine($"║ ✅ Undo Successful! Player {player}       ║");
            Console.WriteLine("║    last move has been reverted.         ║");
        }
        else
        {
            Console.WriteLine($"║ ❌ Undo Failed! Player {player}         ║");
            Console.WriteLine("║    No move to revert.                   ║");
        }

        Console.WriteLine("╚════════════════════════════════════════╝\n");
    }

    public void DisplayHelp(List<(string Name, string Usage, string Description)> commands)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Available Commands ===\n");
        Console.ResetColor();

        var maxNameLength = commands.Count != 0 ? commands.Max(c => c.Name.Length) : 0;

        foreach (var cmd in commands)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var namePadded = cmd.Name.PadRight(maxNameLength);
            Console.Write($"* {namePadded}");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" | {cmd.Description}");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"    Usage: {cmd.Usage}\n");

            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==========================\n");
        Console.ResetColor();
    }

    public void DisplayModeOptions(List<(string DisplayName, string Description)> options)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Select Game ModeType ===\n");
        Console.ResetColor();

        var maxNameLength = options.Count != 0 ? options.Max(o => o.DisplayName.Length) : 0;

        for (var i = 0; i < options.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"{i + 1}. ");

            Console.ForegroundColor = ConsoleColor.Green;
            var namePadded = options[i].DisplayName.PadRight(maxNameLength);
            Console.Write(namePadded);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" - {options[i].Description}");

            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n========================\n");
        Console.ResetColor();
    }

    public void DisplaySetModeResult(string modeName)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"🐷 Mode Selected: {modeName}.");
        Console.ResetColor();
    }

    public void DisplayBotDifficultyOptions(List<(string DisplayName, string Description)> options)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║     SELECT BOT DIFFICULTY          ║");
        Console.WriteLine("╠════════════════════════════════════╣");
        Console.ResetColor();
        var maxNameLength = options.Count != 0 ? options.Max(o => o.DisplayName.Length) : 0;

        for (var i = 0; i < options.Count; i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write($"║  {i + 1}. ");

            Console.ForegroundColor = ConsoleColor.Green;
            var namePadded = options[i].DisplayName.PadRight(maxNameLength);
            Console.Write(namePadded);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" - {options[i].Description}");
            Console.ResetColor();
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╚════════════════════════════════════╝");
        Console.ResetColor();
    }
}