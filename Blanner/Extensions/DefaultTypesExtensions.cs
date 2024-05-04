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

	/// <summary>
	/// Converts the DateOnly to an equivalent DateTime.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static DateTime ToDateTime(this DateOnly value) {
		return value.ToDateTime(TimeOnly.MinValue);
	}

	/// <summary>
	/// Converts the nullable DateTime to an equivalent DateOnly, removing the time part.
	/// Returns <see cref="DateOnly.MinValue"/> if the <paramref name="value"/> is null.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static DateOnly ToDateOnly(this DateTime? value) {
		return value == null ? DateOnly.MinValue : DateOnly.FromDateTime(value.Value);
	}

	/// <summary>
	/// Converts the TimeOnly to an equivalent DateTime.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static DateTime ToDateTime(this TimeOnly value) {
		return new DateTime(value.Ticks);
	}

	/// <summary>
	/// Converts the nullable DateTime to an equivalent TimeOnly, removing the time part.
	/// Returns <see cref="TimeOnly.MinValue"/> if the <paramref name="value"/> is null.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	public static TimeOnly ToTimeOnly(this DateTime? value) {
		return value == null ? TimeOnly.MinValue : TimeOnly.FromDateTime(value.Value);
	}

	public static DateTime Normalize(this DateTime value) {
		return value.Date + new TimeSpan(value.Hour, value.Minute, 0);
	}
}
