using OneOf;
using OneOf.Types;
using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.Engine;

public interface ITaskDispatcher
{
	Task<OneOf<Success, Error<string>>> DispatchNextOperation(TaskMessage taskMessage, IExecutableStep executableStep,
		CancellationToken cancellationToken);
}
