using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskScheduler
{
	Task<TaskResult> DispatchNewOperation(TaskMessage taskMessage, CancellationToken cancellationToken);
}
