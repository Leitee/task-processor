namespace TaskProcessor.Shared.Interfaces;

using System;
using System.Threading;
using System.Threading.Tasks;
using TaskProcessor.Shared;
using TaskProcessor.Shared.Engine;

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
