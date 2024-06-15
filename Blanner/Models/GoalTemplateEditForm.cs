using Blanner.Data;
using Blanner.Hubs;

namespace Blanner.Models;

public class GoalTemplateEditForm() {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int? Contractor { get; set; }

    public bool IsEmpty { get; protected set; }

    public void Set(GoalMainData? data) {
        Id = data?.Id ?? 0;
        Name = data?.Name ?? string.Empty;
        Comment = data?.Comment ?? string.Empty;
        Contractor = data?.Contractor?.Id;

        IsEmpty = data is null;
    }
}
