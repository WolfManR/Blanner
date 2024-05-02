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
		goalsGroup.MapGet("/working", UsersEndpointsBehaviors.WorkingUsers);
	} 
}
public static class UsersEndpointsBehaviors {
	public static async Task<IResult> Users(
		[FromServices] UsersRepository usersRepository) {
		var users = (await usersRepository.Users().ToListAsync()).Select(x => new UserSelectListData(x)).ToList();

		return TypedResults.Json(users);
	}

	public static async Task<IResult> User(
		[FromRoute] string userId,
		[FromServices] UsersRepository usersRepository) {
		var user = await usersRepository.User(userId);

		return user is null ? TypedResults.NotFound() : TypedResults.Json(new UserInfoData(user));
	}

	public static async Task<IResult> WorkingUsers(
		[FromServices] UsersRepository usersRepository) {
		var users = (await usersRepository.WorkingUsers());

		return TypedResults.Json(users);
	}
}

