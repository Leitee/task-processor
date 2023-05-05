using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.IO
{
	public interface IPubSubHandler : ITaskPublisher
	{
		Task<TaskMessage> ConsumeMessageAsync(CancellationToken cancellationToken);
	}
}
