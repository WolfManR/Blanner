using Blanner.Data.DataModels;

using System.Linq;

namespace Blanner.Data.Models;

public static class EntitiesExtensions {
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> self) {
		return self == null || !self.Any();
	}


	public static TimeSpan Time(this ITime self) {
        return self.End <= self.Start ? TimeSpan.Zero : self.End.Subtract(self.Start);
    }

	public static string TimeFormatterValue(this UserSettings? instance) {
		return instance?.TimeFormatter ?? UserSettings.DefaultTimeFormatter;
	}

	public static string DetailedTimeFormatterValue(this UserSettings? instance) {
		return instance?.DetailedTimeFormatter ?? UserSettings.DefaultDetailedTimeFormatter;
	}

	public static string DateTimeFormatterValue(this UserSettings? instance) {
		return instance?.DateTimeFormatter ?? UserSettings.DefaultDateTimeFormatter;
	}

	public static string DetailedDateTimeFormatterValue(this UserSettings? instance) {
		return instance?.DetailedDateTimeFormatter ?? UserSettings.DefaultDetailedDateTimeFormatter;
	}
}
