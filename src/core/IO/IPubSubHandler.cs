using TaskProcessor.Core.Shared.engine;

namespace TaskProcessor.Core.IO
{
	public interface IPubSubHandler
	{
		Task<TaskMessage> ConsumeMessageAsync(CancellationToken cancellationToken);
		Task PublishMessageAsync(TaskMessage message, CancellationToken cancellationToken);
	}
}
