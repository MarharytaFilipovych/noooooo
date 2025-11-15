using System.ComponentModel;
using System.Reflection;

namespace Layout.LayoutType;

public static class LayoutTypeExtensions
{
    public static string GetDescription(this LayoutType value) 
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }
}