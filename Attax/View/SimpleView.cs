using Model.Game.Mode;
using Model.PlayerType;

namespace View;

using Model;
using Model.Game;
using Model.Game.DTOs;
using System;

public class SimpleView : IGameView
{
    public void UpdateBoard(GameState state)
    {
        Console.WriteLine("\nAtaxx - Simple View");
        Console.WriteLine("-------------------");

        Console.Write("  ");
        for (var col = 0; col < state.BoardSize; col++)
        {
            Console.Write($"{(char)('A' + col)} ");
        }

        Console.WriteLine();

        for (var row = 0; row < state.BoardSize; row++)
        {
            Console.Write($"{row + 1} ");
            for (var col = 0; col < state.BoardSize; col++)
            {
                var cell = state.Cells[row, col];
                Console.Write($"{cell.ToSymbol()} ");
            }

            Console.WriteLine();
        }

        Console.WriteLine($"\nScore - X: {state.XCount}, O: {state.OCount}");
    }

    public void DisplayGameStart(GameState state, string layoutName, GameMode mode)
    {
        Console.Clear();
        Console.WriteLine($"Game started with layout: {layoutName}");
        Console.WriteLine($"Mode: {mode}");
        UpdateBoard(state);
    }

    public void DisplayTurn(PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"\n{playerLabel}'s turn");
    }

    public void DisplayMove(Move move, PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"{playerLabel} moved: {move}");
    }

    public void DisplayInvalidMove(Move move) => Console.WriteLine($"Invalid move: {move}");

    public void DisplayGameEnd(GameState state, PlayerType winner)
    {
        UpdateBoard(state);

        Console.WriteLine("\n===================");
        Console.WriteLine(winner == PlayerType.None
            ? "Game ended in a draw!" 
            : $"Player {winner} wins!");

        Console.WriteLine("===================");
    }
}