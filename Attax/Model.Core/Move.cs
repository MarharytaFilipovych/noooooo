namespace Model;

public class Move
{
    public Position From { get; }
    public Position To { get; }
    public MoveType Type { get; }

    public Move(Position from, Position to)
    {
        From = from;
        To = to;
        
        var distance = from.DistanceTo(to);
        
        Type = distance switch
        {
            1 => MoveType.Clone,
            2 => MoveType.Jump,
            _ => MoveType.Invalid
        };
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
