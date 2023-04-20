using OneOf;
using OneOf.Types;
using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.Shared.Interfaces;

public interface ITaskScheduler
{
    Task<OneOf<Success, Error<string>>> DispatchNewOperation(TaskMessage taskMessage, CancellationToken cancellationToken);
}
