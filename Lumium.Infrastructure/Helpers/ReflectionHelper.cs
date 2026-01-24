using System.Reflection;

namespace Lumium.Infrastructure.Helpers;

public static class ReflectionHelper
{
    public static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(
        Type openGenericType, 
        Assembly assembly)
    {
        return from type in assembly.GetTypes()
            from interfaceType in type.GetInterfaces()
            let baseType = type.BaseType
            where
                (baseType is { IsGenericType: true } &&
                 openGenericType.IsAssignableFrom(baseType.GetGenericTypeDefinition())) ||
                (interfaceType.IsGenericType &&
                 openGenericType.IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
            select type;
    }
}