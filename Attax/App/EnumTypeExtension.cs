using System.ComponentModel;
using System.Reflection;

namespace App;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T value) where T : Enum
    {
        var field = typeof(T).GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}