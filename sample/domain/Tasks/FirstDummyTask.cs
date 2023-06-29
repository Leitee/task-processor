using TaskProcessor.Common;
using TaskProcessor.Domain.Operations;
using TaskProcessor.Engine;
using TaskProcessor.Interfaces;

namespace TaskProcessor.Domain.Tasks;

public class FirstDummyTask : IExecutableStep
{
	public string Name => nameof(FirstDummyTask);

	public string OperationName => nameof(DummyWorkflow);

	public byte ExecutionOrder => 1;

	public byte MaxRetries => 2;

	public bool IsLastStep => false;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<ExecutableStepResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000, cancellationToken);
		return ExecutableStepResult.AsSuccess;
	}
}
