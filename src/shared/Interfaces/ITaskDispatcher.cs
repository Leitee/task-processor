using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskDispatcher
{
	Task<TaskResult> DispatchNewOperation(TaskMessage taskMessage, CancellationToken cancellationToken);
}
