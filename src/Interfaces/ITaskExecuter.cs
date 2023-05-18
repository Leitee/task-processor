using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
	public interface ITaskExecuter
	{
		Task<TaskResult> ExecuteNextOperation(TaskMessage taskMessage, IExecutableStep executableStep,
			CancellationToken cancellationToken);
	}
}