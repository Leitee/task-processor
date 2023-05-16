using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Common;
using TaskProcessor.Engine;

namespace TaskProcessor.Interfaces
{
    public interface IExecutableStep
	{
		string Name { get; }
		string OperationName { get; }
		byte ExecutionOrder { get; }
		byte MaxRetires { get; }
		bool IsLastStep { get; }
		TimeSpan Timeout { get; }
		Task<ExecutableStepResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken);
	}
}