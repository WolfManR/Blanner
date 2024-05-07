using Microsoft.AspNetCore.Components;

namespace Blanner.Extensions; 
public static class NavigationManagerExtensions {
	public static Uri GoalsHubUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/hubs/goals");
	}
	public static Uri JobsHubUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/hubs/jobs");
	}
	public static Uri ContractorsHubUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/hubs/contractors");
	}
	public static Uri StickyNotesHubUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/hubs/sticky");
	}
	public static Uri JobsApiUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/api/jobs");
	}
}
