using Blanner.Extensions;
using Blanner.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Blanner.Hubs.Clients;
public interface IStickyNoteClient {
	Task NoteCreated(Note note);
	Task NoteUpdated(Note note);
	Task NoteDeleted(Guid id);
}

public sealed class StickyNoteClient : HubClientBase, IStickyNoteClient, IStickyNoteHub {
	public StickyNoteClient(NavigationManager NavManager) : base(NavManager.StickyNotesHubUri()) {
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
