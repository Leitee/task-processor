using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.IO
{
	public interface IPubSubHandler : ITaskPublisher
	{
		Task<TaskMessage> ConsumeMessageAsync(CancellationToken cancellationToken);
	}
}
