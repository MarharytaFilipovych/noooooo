using System.ComponentModel;
using System.Reflection;

namespace GameMode.ModeType;

public static class GameModeTypeExtensions
{
    public static string GetDescription(this GameModeType value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}