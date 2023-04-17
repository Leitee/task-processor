using TaskProcessor.Core.Abstractions;
using TaskProcessor.Core.Entities;
using TaskProcessor.Core.Models;
using TaskProcessor.Core.Shared.engine;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Engine;

public record TaskSchedulerResponse(string Message, bool HasError = false);

public record TaskSchedulerRequest(MilestoneDto Payload) : IRequest<TaskSchedulerResponse>;

public class TaskScheduler : IRequestHandler<TaskSchedulerRequest, TaskSchedulerResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IMessageBroker _messageBroker;

	public TaskScheduler(IUnitOfWork unitOfWork, IMessageBroker messageBroker)
	{
		_uow = unitOfWork;
		_messageBroker = messageBroker;
	}

	public async Task<TaskSchedulerResponse> Handle(TaskSchedulerRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request.Payload?.Goals);

        var goalsToSave = request.Payload.Goals
                .Select(g => new Goal(g.Name, g.Description));


		var newMilestone = new Milestone(request.Payload.Name)
            .SetGoals(goalsToSave);


        var result = await _uow.GetRepository<Milestone>()
            .InsertAsync(newMilestone, cancellationToken);

        if (!await _uow.CommitAsync(cancellationToken))
            return new TaskSchedulerResponse("Error while saving the milestone", true);

		var messagePayload = new TaskBaseMessage<IPayload>(newMilestone);

        await _messageBroker.PublishAsync(messagePayload, cancellationToken);

        return new TaskSchedulerResponse("OK");
    }
}