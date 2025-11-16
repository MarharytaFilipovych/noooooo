using Command;
using GameMode;

namespace Commands.CommandDefinition;

public interface ICommandDefinition
{
    string Name { get; }
    string Description { get; }
    string Usage { get; }
    bool TryParse(string[] args, out ICommand? command, out string? error);
    bool IsAvailableInMode(ModeType modeType);
}