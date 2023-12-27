
namespace Blanner.Extensions; 
public class Value<TValue, TKey>(TValue? value) : IEquatable<Value<TValue, TKey>?> {
    public TKey Id => KeySelector is not null ? KeySelector.Invoke(StoredValue) : throw new InvalidOperationException($"Key selector for type {this.GetType().Name} not set");

    public TValue? StoredValue { get; set; } = value;

    private static Func<TValue?, TKey>? KeySelector { get; set; }

    public static void SetKeySelector(Func<TValue?, TKey> selector) => KeySelector = selector;

	public override bool Equals(object? obj) => Equals(obj as Value<TValue, TKey>);
	public bool Equals(Value<TValue, TKey>? other) => other is not null && EqualityComparer<TKey>.Default.Equals(Id, other.Id);
	public override int GetHashCode() => HashCode.Combine(Id);

	public static implicit operator TValue?(Value<TValue, TKey> value) => value.StoredValue;

	public static bool operator ==(Value<TValue, TKey>? left, Value<TValue, TKey>? right) {
		return EqualityComparer<Value<TValue, TKey>>.Default.Equals(left, right);
	}

	public static bool operator !=(Value<TValue, TKey>? left, Value<TValue, TKey>? right) {
		return !(left == right);
	}
}
