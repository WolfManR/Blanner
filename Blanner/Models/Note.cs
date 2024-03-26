using Blanner.Hubs.Clients;

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
