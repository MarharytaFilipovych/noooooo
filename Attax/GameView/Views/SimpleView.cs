using Model.Game.DTOs;
using Model.PlayerType;
using Stats;

namespace View.Views;

public class SimpleView : IGameView
{
    public void DisplayWelcome()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Ataxx!!!");
        Console.WriteLine("Type 'help' for available commands");
    }
    
    public void UpdateBoard(GameState state)
    {
        Console.WriteLine("\nAtaxx - Simple GameView");
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

    public void DisplayGameStart(GameState state, string layoutName, string mode)
    {
        Console.Clear();
        Console.WriteLine($"Game started with layout: {layoutName}");
        Console.WriteLine($"ModeType: {mode}");
        UpdateBoard(state);
    }

    public void DisplayTurn(PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"\n{playerLabel}'s turn");
    }

    public void DisplayMove(Move.Move move, PlayerType player, bool isBot)
    {
        var playerLabel = isBot ? $"Bot ({player})" : $"Player {player}";
        Console.WriteLine($"{playerLabel} moved: {move}");
    }

    public void DisplayInvalidMove(Move.Move move) => Console.WriteLine($"Invalid move: {move}");

    public void DisplayGameEnd(GameState state, PlayerType winner)
    {
        UpdateBoard(state);

        Console.WriteLine("\n===================");
        Console.WriteLine(winner == PlayerType.None
            ? "Game ended in a draw!" 
            : $"Player {winner} wins!");

        Console.WriteLine("===================");
    }
    
    public void DisplayHint(List<Move.Move> validMoves)
    {
        Console.WriteLine("\nValid moves:");
        validMoves.ForEach(move => Console.WriteLine($"  {move}"));
    }

    public void DisplayMessage(string message) => Console.WriteLine($" {message}");

    public void DisplayError(string error) =>Console.WriteLine($"Error: {error}");

    public void DisplayStatistics(GameStatistics stats)
    {
        const int labelWidth = 15;
        const int valueWidth = 6;

        Console.WriteLine("\n╔══════════ Game Statistics ══════════╗");
        Console.WriteLine($"│ {"Total Games",-labelWidth} {stats.GamesPlayed,valueWidth} │");
        Console.WriteLine($"│ {"Player X Wins",-labelWidth} {stats.PlayerXWins,valueWidth} │");
        Console.WriteLine($"│ {"Player O Wins",-labelWidth} {stats.PlayerOWins,valueWidth} │");
        Console.WriteLine($"│ {"Draws",-labelWidth} {stats.Draws,valueWidth} │");
        Console.WriteLine($"│ {"Avg Moves",-labelWidth} {stats.AverageMoveCount,valueWidth:F1} │");
        Console.WriteLine($"│ {"Last Played",-labelWidth} {stats.LastPlayed:yyyy-MM-dd} │");
        Console.WriteLine("╚══════════════════════════════════════╝");
    }

    public string DisplayGetInput() 
    {
        Console.Write("> ");
        return Console.ReadLine() ?? string.Empty;
    }
    
    public void DisplayElapsedTimeOutMessage(PlayerType playerType) =>
        Console.WriteLine($"Time's up for {playerType}!" +
                          $" A random move has been made automatically.");
    
    public void DisplayUndo(bool success, PlayerType player)
    {
        Console.WriteLine(success
            ? $"Undo successful! Player {player}'s last move was reverted."
            : $"Undo failed! No move to revert for Player {player}.");
    }
    
    public void DisplayHelp(List<(string Name, string Usage, string Description)> commands)
    {
        Console.WriteLine("Available commands:");
        foreach (var cmd in commands)
        {
            Console.WriteLine($"*{cmd.Name}: {cmd.Description}. Usage: {cmd.Usage}");
        }
    }

    public void DisplayModeOptions(List<(string DisplayName, string Description)> options)
    {
        Console.WriteLine("Select game modeType:");
        for (var i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i].DisplayName} - {options[i].Description}");
        }
    }
    
    public void DisplaySetModeResult(string modeName) =>
        DisplayMessage($"Game mode set to {modeName}.");
}

