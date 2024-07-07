using Blanner.Data.Models;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Security.Principal;


namespace Blanner.Models;

public class UserInfo {
    private readonly AuthenticationStateProvider _authenticationStateProvider;
	private readonly UserManager<User> _userManager;

	private AuthenticationState? _lastState;

    private ClaimsPrincipal? User => _lastState?.User;
    private IIdentity? Identity => User?.Identity;

    public UserInfo(AuthenticationStateProvider authenticationStateProvider, UserManager<User> userManager) {
        _authenticationStateProvider = authenticationStateProvider;
		_userManager = userManager;
	}

	private async ValueTask InitializeAsyncStates() {
        if(_lastState is null)
		    _lastState = await _authenticationStateProvider.GetAuthenticationStateAsync();
	}

	public async ValueTask<string> UserId() {
        await InitializeAsyncStates();

        if (User is null) return string.Empty;
        return _userManager.GetUserId(User) ?? string.Empty;
    }
	public async ValueTask<string> Email() {
        await InitializeAsyncStates();

        return Identity?.Name ?? string.Empty;
    }
}
