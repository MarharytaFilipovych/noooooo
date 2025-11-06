namespace Model;

public readonly struct Position(int row, int col)
{
    public int Row { get; } = row;
    public int Col { get; } = col;

    public int DistanceTo(Position other) =>
        Math.Max(Math.Abs(Row - other.Row), Math.Abs(Col - other.Col));

    public override bool Equals(object? obj) =>
        obj is Position other && (Row == other.Row && Col == other.Col);

    public override int GetHashCode() => Row * 31 + Col;

    public static bool operator ==(Position a, Position b) => a.Equals(b);

    public static bool operator !=(Position a, Position b) => !a.Equals(b);

    public override string ToString()
    {
        var colChar = (char)('A' + Col);
        var rowNum = Row + 1;
        return $"{colChar}{rowNum}";
    }

    public static bool TryParse(string notation, out Position position)
    {
        return PositionParser.TryParse(notation, out position);
    }

    private bool Equals(Position other) => Row == other.Row && Col == other.Col;
}
