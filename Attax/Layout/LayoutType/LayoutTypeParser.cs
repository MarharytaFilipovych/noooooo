namespace Layout.LayoutType;

public static class LayoutTypeParser
{
    public static bool TryParse(string input, out LayoutType type)
    {
        foreach (LayoutType value in Enum.GetValues(typeof(LayoutType)))
        {
            if (value.GetDescription().Equals(input))
            {
                type = value;
                return true;
            }
        }
        type = default;
        return false;
    }
    
    public static string AllValidDescriptions()
    {
        return string.Join(", ", 
            Enum.GetValues(typeof(LayoutType))
                .Cast<LayoutType>()
                .Select(e => e.GetDescription()));
    }
}