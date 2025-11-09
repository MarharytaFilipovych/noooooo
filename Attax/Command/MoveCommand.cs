using Model.Position;

namespace Command;

public class MoveCommand(Position from, Position to) : ICommand
{
    public Position From { get; } = from;
    public Position To { get; } = to;
}
