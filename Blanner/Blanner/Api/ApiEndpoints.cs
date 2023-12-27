namespace Blanner.Api {
	public static class ApiEndpoints {
	    public static IEndpointRouteBuilder MapRoutes(this IEndpointRouteBuilder endpoints) {
	        var apiGroup = endpoints.MapGroup("/api");

			apiGroup.MapGoals();
			apiGroup.MapContractors();
			apiGroup.MapJobs();

	        return endpoints;
	    }
	}
}
