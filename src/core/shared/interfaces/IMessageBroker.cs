using TaskProcessor.Core.Engine;

namespace TaskProcessor.Core.Shared;

public interface IMessageBroker
{
    Task<TaskMessageBase<IPayload>> DequeueAsync(CancellationToken cancellationToken = default);
    Task PublishAsync(TaskMessageBase<IPayload> taskMessage, CancellationToken cancellationToken = default);
}