using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Shared.Engine;

namespace TaskProcessor.Shared.Interfaces
{
	public interface IExecutableStep
	{
		string Name { get; }
		string OperationName { get; }
		byte ExecutionOrder { get; }
		byte MaxRetires { get; }
		bool IsLastStep { get; }
		TimeSpan Timeout { get; }
		Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken);
	}
}