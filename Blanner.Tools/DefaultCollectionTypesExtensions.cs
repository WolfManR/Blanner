namespace System.Collections.Generic;
public static class DefaultCollectionTypesExtensions {
	public static bool IsNullOrEmpty<T>(this IEnumerable<T> self) {
		return self == null || !self.Any();
	}

	public static void Change<T>(this IEnumerable<T> collection, Action<T> changeBehavior) {
		foreach (var item in collection) {
			changeBehavior.Invoke(item);
		}
	}
}
