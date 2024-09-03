using Blanner.Data;
using Blanner.Data.Models;
using Blanner.Hubs;

namespace Blanner.Models;

public class GoalTemplateEditForm() : GoalBaseEditForm {
    public HashSet<Contractor> Contractors { get; set; } = [];

    public void Set(GoalTemplateDetailsData? data) {
        Id = data?.Id ?? 0;
        Name = data?.Name ?? string.Empty;
        Comment = data?.Comment ?? string.Empty;
        Contractors = data?.Contractors.ToHashSet() ?? [];

        IsEmpty = data is null;
    }
}
