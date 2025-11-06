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

        Console.Write("   ");
        for (var col = 0; col < state.BoardSize; col++)
        {
            Console.Write($" {(char)('A' + col)} ");
            if (col < state.BoardSize - 1) Console.Write(" ");
        }

        Console.WriteLine();

        Console.Write("  ┌");
        for (var col = 0; col < state.BoardSize; col++)
        {
            Console.Write("───");
            if (col < state.BoardSize - 1) Console.Write("┬");
        }

        Console.WriteLine("┐");

        for (var row = 0; row < state.BoardSize; row++)
        {
            Console.Write($"{row + 1} │");
            for (var col = 0; col < state.BoardSize; col++)
            {
                var cell = state.Cells[row, col];
                var symbol = GetCellSymbol(cell);
                Console.Write($" {symbol} ");
                if (col < state.BoardSize - 1) Console.Write("│");
            }

            Console.WriteLine("│");

            if (row < state.BoardSize - 1)
            {
                Console.Write("  ├");
                for (var col = 0; col < state.BoardSize; col++)
                {
                    Console.Write("───");
                    if (col < state.BoardSize - 1) Console.Write("┼");
                }

                Console.WriteLine("┤");
            }
        }

        Console.Write("  └");
        for (var col = 0; col < state.BoardSize; col++)
        {
            Console.Write("───");
            if (col < state.BoardSize - 1) Console.Write("┴");
        }

        Console.WriteLine("┘");

        Console.WriteLine($"\nScore │ X: {state.XCount} │ O: {state.OCount}");
    }

    public void DisplayGameStart(GameState state, string layoutName, GameMode mode)
    {
        Console.Clear();
        Console.WriteLine("════════════════════════════════");
        Console.WriteLine($"    Game started");
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

    public void DisplayInvalidMove(Move move)
    {
        Console.WriteLine($" Invalid move: {move}");
    }

    public void DisplayGameEnd(GameState state, PlayerType winner)
    {
        UpdateBoard(state);

        Console.WriteLine("\n════════════════════════════════");
        if (winner == PlayerType.None)
        {
            Console.WriteLine("    Game ended in a draw!");
        }
        else
        {
            Console.WriteLine($"    Player {winner} wins!");
        }

        Console.WriteLine("════════════════════════════════");
    }

    public void DisplayMessage(string message)
    {
        Console.WriteLine($" {message}");
    }

    public string GetInput(string prompt)
    {
        Console.Write($"> {prompt}: ");
        return Console.ReadLine() ?? string.Empty;
    }

    private static char GetCellSymbol(CellState cell)
    {
        if (cell.IsBlocked) return '#';
        if (!cell.IsOccupied) return ' ';
        return cell.OccupiedBy == PlayerType.X ? 'X' : 'O';
    }
}