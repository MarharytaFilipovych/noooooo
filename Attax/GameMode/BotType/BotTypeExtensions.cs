using System.ComponentModel;
using System.Reflection;
using GameMode.ModeConfigurations;

namespace GameMode.BotType;

public static class BotTypeExtensions
{
    public static string GetDescription(this BotDifficulty value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}