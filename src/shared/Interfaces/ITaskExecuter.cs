using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces;

public interface ITaskExecuter
{
	Task<TaskResult> ExecuteNextOperation(TaskMessage taskMessage, IExecutableStep executableStep,
		CancellationToken cancellationToken);
}
