using OneOf;
using OneOf.Types;
using TaskProcessor.Core.Abstractions;
using TaskProcessor.Core.IO;
using TaskProcessor.Core.Shared.engine;
using TaskProcessor.Core.Shared.Interfaces;

namespace TaskProcessor.Core.Engine;

public class TaskScheduler : ITaskScheduler
{
	private readonly IUnitOfWork _uow;
	private readonly IPubSubHandler _messageBroker;

	public TaskScheduler(IUnitOfWork unitOfWork, IPubSubHandler messageBroker)
	{
		_uow = unitOfWork;
		_messageBroker = messageBroker;
	}

	public async Task<OneOf<Success, Error<string>>> DispatchNewOperation(TaskMessage taskMessage,
		CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(taskMessage);

		var newTaskMessage = await _uow.GetTasksStore()
			.InsertAsync(taskMessage, cancellationToken);

		if (!await _uow.CommitAsync(cancellationToken))
			return new Error<string>("Error while saving the milestone");

		await _messageBroker.PublishMessageAsync(newTaskMessage, cancellationToken);

		return new Success();
	}
}