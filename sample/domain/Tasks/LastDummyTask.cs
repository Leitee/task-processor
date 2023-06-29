using TaskProcessor.Common;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Tasks;

public class LastDummyTask : IExecutableStep
{
	public string Name => nameof(LastDummyTask);

	public string OperationName => nameof(DummyWorkflow);

	public byte ExecutionOrder => byte.MaxValue;

	public byte MaxRetries => 1;

	public bool IsLastStep => true;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<ExecutableStepResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000, cancellationToken);
		return ExecutableStepResult.AsSuccess;
	}
}
