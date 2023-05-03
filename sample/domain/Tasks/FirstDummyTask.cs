using TaskProcessor.Domain.Operations;
using TaskProcessor.Shared;
using TaskProcessor.Shared.Engine;
using TaskProcessor.Shared.Interfaces;

namespace TaskProcessor.Domain.Tasks;

public class FirstDummyTask : IExecutableStep
{
	public string Name => nameof(FirstDummyTask);

	public string OperationName => nameof(EnrollStudentOperation);

	public byte ExecutionOrder => 1;

	public byte MaxRetires => 2;

	public bool IsLastStep => false;

	public TimeSpan Timeout => TimeSpan.FromSeconds(10);

	public async Task<TaskResult> ExecuteAsync(TaskMessage taskMessage, CancellationToken cancellationToken)
	{
		await Task.Delay(1000);
		return TaskResult.Success;
	}
}
