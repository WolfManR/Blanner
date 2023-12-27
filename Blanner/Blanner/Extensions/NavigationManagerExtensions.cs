using Microsoft.AspNetCore.Components;

namespace Blanner.Extensions; 
public static class NavigationManagerExtensions {
	public static Uri GoalsHubUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/hubs/goals");
	}
	public static Uri JobsApiUri(this NavigationManager navManager) {
		return navManager.ToAbsoluteUri("/api/jobs");
	}
}
