namespace Model.Core;

public class Move
{
    public Position From { get; }
    public Position To { get; }
    public MoveType Type { get; }

    public Move(Position from, Position to)
    {
        From = from;
        To = to;
        
        int distance = from.DistanceTo(to);
        if (distance == 1)
        {
            Type = MoveType.Clone;
        }
        else if (distance == 2)
        {
            Type = MoveType.Jump;
        }
        else
        {
            Type = MoveType.Invalid;
        }
    }

    public bool IsValid => Type != MoveType.Invalid;

    public override string ToString()
    {
        return $"{From} -> {To} ({Type})";
    }
}

public enum MoveType
{
    Invalid,
    Clone,
    Jump
}
