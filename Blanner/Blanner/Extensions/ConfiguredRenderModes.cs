using Microsoft.AspNetCore.Components.Web;

namespace Blanner.Extensions {
	public static class ConfiguredRenderModes {
		public static InteractiveServerRenderMode InteractiveServerNotPrerendered { get; } = new(prerender: false);
	}
}
