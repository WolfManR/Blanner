using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;

public class HubClientBase : IAsyncDisposable {
	protected HubClientBase(Uri hubPath) =>
		Hub = new HubConnectionBuilder()
			.WithUrl(hubPath)
			.WithAutomaticReconnect()
			.Build();

	protected bool Started { get; private set; }
	protected HubConnection Hub { get; private set; }

	public bool IsConnected => Hub.State == HubConnectionState.Connected;

	public string? ConnectionId => Hub.ConnectionId;

	public async ValueTask DisposeAsync() {
		await Hub.DisposeAsync();
		GC.SuppressFinalize(this);
	}

	public async Task Start() {
		if (!Started) {
			await Hub.StartAsync();
			Started = true;
		}
	}
}
