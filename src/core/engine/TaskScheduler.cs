using TaskProcessor.Core.Models;
using TaskProcessor.Core.Shared;

namespace TaskProcessor.Core.Engine;

public record TaskSchedulerResponse(string Message, bool HasError = false);

public record TaskSchedulerRequest(MilestoneDto payload) : IRequest<TaskSchedulerResponse>;

public class TaskScheduler : IRequestHandler<TaskSchedulerRequest, TaskSchedulerResponse>
{
    private readonly IUnitOfWork _uow;

    public TaskScheduler(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<TaskSchedulerResponse> Handle(TaskSchedulerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.payload);

        var newMilestone = new Milestone(request.payload.Name);
           // .SetGoals(request.payload.Goals.Select(new GoalDto{ }))
 
        var result = await _uow.GetRepository<Milestone>().InsertAsync(newMilestone, cancellationToken);

        return new TaskSchedulerResponse("OK");
    }
}