using TaskProcessor.Core.Engine;

namespace TaskProcessor.Core.Shared;

public interface IMessageBroker
{
    Task<TaskMessageBase> DequeueAsync(CancellationToken cancellationToken = default);
    Task PublishAsync(TaskMessageBase taskMessage, CancellationToken cancellationToken = default);
}