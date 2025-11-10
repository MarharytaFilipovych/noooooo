namespace Move;

public class Move
{
    public Model.Position.Position From { get; }
    public Model.Position.Position To { get; }
    public MoveType Type { get; }

    public Move(Model.Position.Position from, Model.Position.Position to)
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
