using System.Diagnostics.CodeAnalysis;

namespace Blanner.Extensions;

public static class DefaultTypesExtensions {
	public static bool NullOrEmpty([NotNullWhen(false)]this string? value) => string.IsNullOrEmpty(value);
}
