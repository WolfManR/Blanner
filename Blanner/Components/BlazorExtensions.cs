using Microsoft.AspNetCore.Components;

namespace Blanner.Components;

public static class BlazorExtensions {
	public static TValue Value<TValue>(this ChangeEventArgs e, TValue defaultValue) {
		return e.Value is null ? defaultValue : (TValue) Convert.ChangeType(e.Value, typeof(TValue));
	}
}
