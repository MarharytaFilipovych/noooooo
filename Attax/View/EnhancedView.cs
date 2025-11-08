using Model.Game.Mode;
using Model.PlayerType;

namespace View;

using Model;
using Model.Game;
using Model.Game.DTOs;
using System;

public class EnhancedView : IGameView
{
    public void UpdateBoard(GameState state)
    {
        Console.WriteLine("\nAtaxx - Enhanced View");
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
}