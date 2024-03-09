using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Models;

public class Note {
	private CancellationTokenSource? _cts;

	public Note() { }

	public Note(double x, double y) {
		Id = Guid.NewGuid();
		LastEdited = DateTimeOffset.UtcNow;
		X = x;
		Y = y;
	}

	public Guid Id { get; set; }
	public string Text { get; set; } = string.Empty;
	public double X { get; set; }
	public double Y { get; set; }
	public string? LastLockingUser { get; set; }
	public DateTimeOffset LastEdited { get; set; }

	public bool CanLock(string? connectionId) =>
		DateTimeOffset.UtcNow.Subtract(LastEdited).TotalSeconds > 1
		|| LastLockingUser is null
		|| LastLockingUser == connectionId;

	public void Lock(string? connectionId) {
		LastLockingUser = connectionId;
		LastEdited = DateTimeOffset.UtcNow;
	}

	public bool TryLock(string? connectionId, IStickyNoteClient? others = default) {
		lock (this) {
			if (!CanLock(connectionId)) return false;
			Lock(connectionId);

			_cts?.Cancel();
			if (others is null) return true;

			_cts = new CancellationTokenSource();
			ThreadPool.QueueUserWorkItem(new WaitCallback(async parameter => {
				CancellationToken token = (CancellationToken) parameter!;
				// ReSharper disable once MethodSupportsCancellation
				await Task.Delay(1000);
				if (token.IsCancellationRequested) return;
				await others.NoteUpdated(this);
			}), _cts.Token);
			return true;
		}
	}
}
public interface IStickyNoteClient {
	Task NoteCreated(Note note);
	Task NoteUpdated(Note note);
	Task NoteDeleted(Guid id);
}

public interface IStickyNoteHub {
	Task<List<Note>> LoadNotes();
	Task CreateNote(double x, double y);
	Task UpdateNoteText(Guid id, string text);
	Task<bool> LockNote(Guid id);
	Task MoveNote(Guid id, double x, double y);
	Task ClearNotes();
	Task DeleteNote(Guid id);
}
public class HubClientBase : IAsyncDisposable {
	protected HubClientBase(string hubPath) =>
		Hub = new HubConnectionBuilder()
			.WithUrl(hubPath)
			.WithAutomaticReconnect()
			.Build();

	protected bool Started { get; private set; }
	protected HubConnection Hub { get; private set; }

	public bool IsConnected =>
		Hub.State == HubConnectionState.Connected;

	public string? ConnectionId => Hub.ConnectionId;

	public async ValueTask DisposeAsync() {
		await Hub.DisposeAsync();
	}

	public async Task Start() {
		if (!Started) {
			await Hub.StartAsync();
			Started = true;
		}
	}
}
public class StickyNoteClient : HubClientBase, IStickyNoteClient, IStickyNoteHub {
	public StickyNoteClient(NavigationManager NavManager) : base(NavManager.ToAbsoluteUri("/hubs/sticky").ToString()) {
		Hub.On<Note, Task>(nameof(NoteCreated), NoteCreated);
		Hub.On<Note, Task>(nameof(NoteUpdated), NoteUpdated);
		Hub.On<Guid, Task>(nameof(NoteDeleted), NoteDeleted);
	}

	public event Func<Note, Task>? OnNoteCreated;
	public event Func<Note, Task>? OnNoteUpdated;
	public event Func<Guid, Task>? OnNoteDeleted;

	public async Task NoteCreated(Note note) {
		if (OnNoteCreated is not null)
			await OnNoteCreated.Invoke(note);
	}

	public async Task NoteUpdated(Note note) {
		if (OnNoteUpdated is not null)
			await OnNoteUpdated.Invoke(note);
	}

	public async Task NoteDeleted(Guid id) {
		if (OnNoteDeleted is not null)
			await OnNoteDeleted.Invoke(id);
	}

	public async Task<List<Note>> LoadNotes() {
		return await Hub.InvokeAsync<List<Note>>(nameof(LoadNotes));
	}

	public async Task CreateNote(double x, double y) {
		await Hub.SendAsync(nameof(CreateNote), x, y);
	}

	public async Task UpdateNoteText(Guid id, string text) {
		await Hub.SendAsync(nameof(UpdateNoteText), id, text);
	}

	public async Task<bool> LockNote(Guid id) {
		return await Hub.InvokeAsync<bool>(nameof(LockNote), id);
	}

	public async Task MoveNote(Guid id, double x, double y) {
		await Hub.SendAsync(nameof(MoveNote), id, x, y);
	}

	public async Task ClearNotes() {
		await Hub.SendAsync(nameof(ClearNotes));
	}

	public async Task DeleteNote(Guid id) {
		await Hub.SendAsync(nameof(DeleteNote), id);
	}
}