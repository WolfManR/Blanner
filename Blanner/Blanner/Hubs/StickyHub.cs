using Blanner.Models;
using Microsoft.AspNetCore.SignalR;

namespace Blanner.Hubs;

public class StickyHub : Hub<IStickyNoteClient>, IStickyNoteHub {
	public Task<List<Note>> LoadNotes() {
		return Task.FromResult(StaticStorage.Notes);
	}

	public async Task CreateNote(double x, double y) {
		Note newNote = new(x, y);
		StaticStorage.Notes.Add(newNote);
		await Clients.All.NoteCreated(newNote);
	}

	public async Task UpdateNoteText(Guid id, string text) {
		if (StaticStorage.Notes.FirstOrDefault(n => n.Id == id) is not { } serverNote) return;
		if (!serverNote.TryLock(Context.ConnectionId, Clients.Others)) {
			await Clients.Caller.NoteUpdated(serverNote);
			return;
		}

		serverNote.Text = text;
		await Clients.Others.NoteUpdated(serverNote);
	}

	public async Task<bool> LockNote(Guid id) {
		if (StaticStorage.Notes.FirstOrDefault(n => n.Id == id) is not { } serverNote) return false;

		if (!serverNote.TryLock(Context.ConnectionId, Clients.Others)) return false;

		await Clients.Others.NoteUpdated(serverNote);
		return true;
	}

	public async Task MoveNote(Guid id, double x, double y) {
		if (StaticStorage.Notes.FirstOrDefault(m => m.Id == id) is not { } serverNote) return;
		if (!serverNote.TryLock(Context.ConnectionId, Clients.Others)) {
			await Clients.Caller.NoteUpdated(serverNote);
			return;
		}

		serverNote.X = x;
		serverNote.Y = y;
		await Clients.Others.NoteUpdated(serverNote);
	}

	public async Task ClearNotes() {
		var noteIds = StaticStorage.Notes.Select(n => n.Id).ToList();
		foreach (Guid id in noteIds) {
			await DeleteNote(id);
		}
	}

	public async Task DeleteNote(Guid id) {
		if (StaticStorage.Notes.FirstOrDefault(o => o.Id == id) is not { } serverNote) return;

		if (!serverNote.TryLock(Context.ConnectionId)) return;

		StaticStorage.Notes.Remove(serverNote);
		await Clients.All.NoteDeleted(id);
	}
}

public static class StaticStorage {
	public static List<Note> Notes { get; set; } = new();
}