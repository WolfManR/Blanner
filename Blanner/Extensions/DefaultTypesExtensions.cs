using System.Diagnostics.CodeAnalysis;

namespace Blanner.Extensions;

public static class DefaultTypesExtensions {
	public static bool NullOrEmpty([NotNullWhen(false)]this string? value) => string.IsNullOrEmpty(value);
	public static void Change<T>(this IEnumerable<T> collection, Action<T> changeBehavior) {
        foreach (var item in collection)
        {
            changeBehavior.Invoke(item);
        }
    }
}
