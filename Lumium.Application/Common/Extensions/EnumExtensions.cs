using System.ComponentModel;
using System.Reflection;

namespace Lumium.Application.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        
        if (field == null)
            return value.ToString();

        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
        
        return attribute?.Description ?? value.ToString();
    }

    public static List<EnumItem<T>> GetEnumItems<T>() where T : struct, Enum
    {
        return Enum.GetValues<T>()
            .Select(value => new EnumItem<T>
            {
                Value = value,
                Name = value.ToString(),
                Description = value.GetDescription()
            })
            .ToList();
    }
}

public class EnumItem<T> where T : Enum
{
    public T Value { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}