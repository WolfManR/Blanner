using Blanner.Data;
using Blanner.Data.DataModels;
using Blanner.Hubs;
using Coravel.Invocable;

using Microsoft.AspNetCore.SignalR;

namespace Blanner.Services.Jobs;
public class TimeNotifier(ActiveGoalsRepository repository, IHubContext<GoalsHub, IGoalsHub> goalsHubContext) : IInvocable
{
	private readonly ActiveGoalsRepository _repository = repository;
	private readonly IHubContext<GoalsHub, IGoalsHub> _goalsHubContext = goalsHubContext;

    public async Task Invoke()
    {
        List<ActiveTimerData> elapsedTime = await _repository.ActiveTimers();
        await Parallel.ForEachAsync(elapsedTime, async (elapsed, token) =>
        {
            await _goalsHubContext.Clients.All.Tick(elapsed.GoalId, elapsed.TimerId, elapsed.UserId);
        });
    }
}
