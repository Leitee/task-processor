using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.Shared.Interfaces;

public interface IMessageBroker
{
	Task<TaskBaseMessage<IPayload>> DequeueAsync(CancellationToken cancellationToken = default);
	Task PublishAsync(TaskBaseMessage<IPayload> taskMessage, CancellationToken cancellationToken = default);
}