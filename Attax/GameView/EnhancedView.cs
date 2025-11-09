using Model.Game.Mode;
using Model.PlayerType;
using Stats;

namespace View;

using Model;
using Model.Game.DTOs;
using System;

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

    public void DisplayGameStart(GameState state, string layoutName, GameMode mode)
    {
        Console.Clear();
        Console.WriteLine("════════════════════════════════");
        Console.WriteLine("    Game started");
        Console.WriteLine($"    Layout: {layoutName}");
        Console.WriteLine($"    Mode: {mode}");
        Console.WriteLine("════════════════════════════════");
        UpdateBoard(state);
    }

    public void DisplayTurn(PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"\n> {playerLabel}'s turn");
    }

    public void DisplayMove(Move move, PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($" {playerLabel} moved: {move}");
    }

    public void DisplayInvalidMove(Move move) => Console.WriteLine($" Invalid move: {move}");

    public void DisplayGameEnd(GameState state, PlayerType winner)
    {
        UpdateBoard(state);

        Console.WriteLine("\n════════════════════════════════");
        Console.WriteLine(winner == PlayerType.None 
            ? "    Game ended in a draw!"
            : $"    Player {winner} wins!");


        Console.WriteLine("════════════════════════════════");
    }
    
    public void DisplayHint(List<Move> validMoves)
    {
        Console.WriteLine("╔══════════════════════════════╗");
        Console.WriteLine("║       Valid Moves            ║");
        Console.WriteLine("╚══════════════════════════════╝");
        validMoves.ForEach(move => Console.WriteLine($"  → {move}"));
    }

    public void DisplayMessage(string message) => 
        Console.WriteLine($"❤️❤️❤️❤️❤️❤️ {message}");
    
    public string GetInput()
    {
        Console.Write("❤️ ");
        return Console.ReadLine() ?? string.Empty;
    }


    public string DisplayMessageForAnswer(string message)
    {
        Console.Write($"❤️❤️❤️❤️❤️❤️ {message}");
        return Console.ReadLine() ?? string.Empty;
    } 
        
    
    public void DisplayError(string error)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine($"OHHHHH NOOOOOOOOOOO🥺🥺🥺🥺🥺: {error}");
        Console.ResetColor();
    }

    public string DisplayModeSelection()
    {
        Console.WriteLine("🐷🐷🐷Select game mode:");
        Console.WriteLine("1 - Player vs Player🤗");
        Console.WriteLine("2 - Player vs Bot👀");
        
        Console.Write("Enter choice (1 or 2)❤️❤️❤️");

        return Console.ReadLine() ?? string.Empty;
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
}

