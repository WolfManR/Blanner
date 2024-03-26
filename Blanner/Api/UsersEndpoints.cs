using Blanner.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blanner.Api;

public static class UsersEndpoints {
	public static void MapUsers(this IEndpointRouteBuilder endpoints) {
		ArgumentNullException.ThrowIfNull(endpoints);

		var goalsGroup = endpoints.MapGroup("/users");

		goalsGroup.MapGet("/", UsersEndpointsBehaviors.Users);
		goalsGroup.MapGet("/{userId}", UsersEndpointsBehaviors.User);
	} 
}
public static class UsersEndpointsBehaviors {
	public static async Task<IResult> Users(
		[FromServices] UsersRepository usersRepository) {
		var goals = (await usersRepository.Users().ToListAsync()).Select(x => new UserSelectListData(x)).ToList();

		return TypedResults.Json(goals);
	}

	public static async Task<IResult> User(
		[FromRoute] string userId,
		[FromServices] UsersRepository usersRepository) {
		var goal = await usersRepository.User(userId);

		return goal is null ? TypedResults.NotFound() : TypedResults.Json(new UserInfoData(goal));
	}
}

