namespace Model;

public static class PositionParser
{
    public static bool TryParse(string notation, out Position position)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(notation) || notation.Length < 2)
            {
                throw new ArgumentException("Invalid position notation");
            }

            var colChar = char.ToUpper(notation[0]);
            var rowStr = notation.Substring(1);

            if (colChar is < 'A' or > 'Z') 
                throw new ArgumentException("Column must be a letter");

            if (!int.TryParse(rowStr, out int row))
                throw new ArgumentException("Row must be a number");

            var col = colChar - 'A';
            position  = new Position(row - 1, col);
            return true;
        }
        catch
        {
            position = default;
            return false;
        }
    }

}