using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskDispatcher
{
	Task<TaskResult> DispatchNextOperation(TaskMessage taskMessage, IExecutableStep executableStep,
		CancellationToken cancellationToken);
}
