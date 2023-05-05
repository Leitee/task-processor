using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces;

public interface ITaskDispatcher
{
	Task<TaskResult> DispatchNewOperation(TaskMessage taskMessage, CancellationToken cancellationToken);
}
