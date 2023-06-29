using System.Threading.Tasks;
using System.Threading;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
	public interface IFailureStep
	{
		bool IsDeadLetter { get; }

		Task<TaskResult> HandleFailureAsync(TaskMessage taskMessage, string errorMsg, short code, CancellationToken cancellationToken);
	}
}