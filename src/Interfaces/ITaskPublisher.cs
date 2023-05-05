using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
    public interface ITaskPublisher
	{
		Task<TaskResult> PublishMessageAsync(TaskMessage message, CancellationToken cancellationToken);
	}
}