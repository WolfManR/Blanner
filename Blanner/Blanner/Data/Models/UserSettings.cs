namespace Blanner.Data.Models; 
public class UserSettings
{
    public int Id { get; set; }

    public const string DefaultTimeFormatter = @"hh\:mm";
    public const string DefaultDetailedTimeFormatter = @"hh\:mm\:ss";

    public User? User { get; set; }
    public string TimeFormatter { get; set; } = DefaultTimeFormatter;
    public string DetailedTimeFormatter { get; set; } = DefaultDetailedTimeFormatter;
}
