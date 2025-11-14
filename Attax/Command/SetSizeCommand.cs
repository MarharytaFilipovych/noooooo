namespace Command;

public class SetSizeCommand(int size) : ICommand
{
    public int Size { get; } = size;
}