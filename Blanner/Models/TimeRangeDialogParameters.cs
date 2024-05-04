using Blanner.Components.Shared.Models;

using Microsoft.AspNetCore.Components;

namespace Blanner.Models;

public class TimeRangeDialogParameters {
	public TimeRange? TimeRange { get; set; }
    public bool StartEditable { get; set; } = true;
    public bool EndEditable { get; set; } = true;
}
