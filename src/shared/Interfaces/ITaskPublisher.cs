using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskPublisher
{
	Task<TaskResult> PublishMessageAsync(TaskMessage message, CancellationToken cancellationToken);
}
