using Blanner.Data.Models.WorksManagement;

namespace Blanner.Data.Models;
public class GoalsGroup {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Goal> Goals { get; set; } = [];
}
