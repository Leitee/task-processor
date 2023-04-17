using TaskProcessor.Core.Models;
using TaskProcessor.Core.Shared;

namespace TaskProcessor.Core.Engine;

public record TaskSchedulerResponse(string Message, bool HasError) : TraceableBase;

public record TaskSchedulerRequest(PayloadDto payload) : IRequest<TaskSchedulerResponse>;

public class TaskScheduler : IRequestHandler<TaskSchedulerRequest, TaskSchedulerResponse>
{
    private readonly IUnitOfWork _uow;

    public TaskScheduler(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<TaskSchedulerResponse> Handle(TaskSchedulerRequest request, CancellationToken cancellationToken)
    {
        await _uow.GetRepository<Milestone>().

        return Task.CompletedTask;
    }
}