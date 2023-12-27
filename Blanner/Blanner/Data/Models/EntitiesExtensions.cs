﻿using Blanner.Data.DataModels;

namespace Blanner.Data.Models;

public static class EntitiesExtensions {
    public static TimeSpan Time(this ITime self) {
        return self.End <= self.Start ? TimeSpan.Zero : self.End.Subtract(self.Start);
    }

	public static string TimeFormatterValue(this UserSettings? instance) {
		return instance?.TimeFormatter ?? UserSettings.DefaultTimeFormatter;
	}

	public static string DetailedTimeFormatterValue(this UserSettings? instance) {
		return instance?.DetailedTimeFormatter ?? UserSettings.DefaultDetailedTimeFormatter;
	}
}
