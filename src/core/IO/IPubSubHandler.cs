using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Core.IO
{
	public interface IPubSubHandler : ITaskPublisher
	{
		Task<TaskMessage> ConsumeMessageAsync(CancellationToken cancellationToken);
	}
}
