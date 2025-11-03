namespace Model.Core;

using System;

public struct Position
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public static Position Parse(string notation)
    {
        if (string.IsNullOrWhiteSpace(notation) || notation.Length < 2)
        {
            throw new ArgumentException("Invalid position notation");
        }

        var colChar = char.ToUpper(notation[0]);
        var rowStr = notation.Substring(1);

        if (colChar < 'A' || colChar > 'Z')
        {
            throw new ArgumentException("Column must be a letter");
        }

        if (!int.TryParse(rowStr, out int row))
        {
            throw new ArgumentException("Row must be a number");
        }

        int col = colChar - 'A';
        return new Position(row - 1, col);
    }

    public string ToNotation()
    {
        char colChar = (char)('A' + Col);
        int rowNum = Row + 1;
        return $"{colChar}{rowNum}";
    }

    public int DistanceTo(Position other)
    {
        return Math.Max(Math.Abs(Row - other.Row), Math.Abs(Col - other.Col));
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Position))
            return false;

        var other = (Position)obj;
        return Row == other.Row && Col == other.Col;
    }

    public override int GetHashCode()
    {
        return Row * 31 + Col;
    }

    public static bool operator ==(Position a, Position b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Position a, Position b)
    {
        return !a.Equals(b);
    }

    public override string ToString()
    {
        return ToNotation();
    }
}
