namespace Blanner.Data.DataModels;

public interface ITime
{
    DateTimeOffset Start { get; set; }
    DateTimeOffset End { get; set; }
}