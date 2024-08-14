namespace Blanner.Data.DataModels;
public class TimeRange : ITime {
	public DateTimeOffset Start { get; set; }
	public DateTimeOffset End { get; set; }
}